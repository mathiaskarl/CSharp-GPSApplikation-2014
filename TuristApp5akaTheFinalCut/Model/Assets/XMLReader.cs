using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using TuristApp5akaTheFinalCut.Common;

namespace Tracker.Model
{
    class XMLReader
    {
        private IStorageFolder StorageFolder = KnownFolders.DocumentsLibrary;
        private IStorageFile TempStorageFile;
        private IStorageFolder TempStorageFolder;

        private Dictionary<string, List<string>> FolderLocations;
        private Dictionary<string, List<string>> FolderLocationsRead;
        private Object TempObject;

        private ObservableCollection<object> ObjectList = new ObservableCollection<object>();


        // Skriv enkelt objekt til XML - Finder selv folder via containedInObject.
        public async Task ObjectToXML(Object obj, Object containedInObject = null)
        {
            await FolderLocation(obj, containedInObject);
            TempStorageFile = await StorageFolder.CreateFileAsync(obj.GetType().Name + "_" + Convert.ToInt16(obj.GetType().GetRuntimeProperty("Id").GetValue(obj)) + ".xml", CreationCollisionOption.ReplaceExisting);
            await WriteFile(obj);
            StorageFolder = KnownFolders.DocumentsLibrary;
        }


        // Skriver liste af objekter til XML - Finder selv folder via containedInObject.
        public async Task ListToXML(IEnumerable<Object> objList, Object containedInObject = null)
        {
            foreach (Object obj in objList)
            {
                await FolderLocation(obj, containedInObject);
                // Opret XML
                TempStorageFile = await StorageFolder.CreateFileAsync(obj.GetType().Name + "_" + Convert.ToInt16(obj.GetType().GetRuntimeProperty("Id").GetValue(obj)) + ".xml", CreationCollisionOption.ReplaceExisting);

                // Skriv til XML fil.
                await WriteFile(obj);
                StorageFolder = KnownFolders.DocumentsLibrary;
            }

        }

        // Sletter XML tilhørende objekt
        public async Task RemoveObject(Object obj, Object containedInObject = null)
        {
            await FolderLocation(obj, containedInObject);
            TempStorageFile = await StorageFolder.GetFileAsync(obj.GetType().Name + "_" + Convert.ToInt32(obj.GetType().GetRuntimeProperty("Id").GetValue(obj)) + ".xml");
            if (TempStorageFile != null)
                await TempStorageFile.DeleteAsync();
            StorageFolder = KnownFolders.DocumentsLibrary;
        }

        // Sletter XML mappe tilhørende objekt
        public async Task RemoveObjectFolder(Object obj, Object containedInObject = null)
        {
            await FolderLocation(obj, containedInObject);
            if (StorageFolder != null)
            {
                foreach (var folders in (await StorageFolder.GetFoldersAsync()))
                {
                    foreach (var file in (await folders.GetFilesAsync()))
                    {
                        file.DeleteAsync();
                    }
                    folders.DeleteAsync();
                }
                foreach (var file in (await StorageFolder.GetFilesAsync()))
                {
                    file.DeleteAsync();
                }
                StorageFolder.DeleteAsync();
            }
            StorageFolder = KnownFolders.DocumentsLibrary;
        }

        // Finder og klargør skrivning af XML fil til dynamisk mappe.
        private async Task FolderLocation(Object obj, Object containedInObject = null, bool ReadWrite = false)
        {
            var FoldersToRead = (ReadWrite ? FolderLocationsRead : FolderLocations);
            StorageFolder = await StorageFolder.CreateFolderAsync("Database", CreationCollisionOption.OpenIfExists);
            foreach (KeyValuePair<string, List<string>> dictionary in FoldersToRead)
            {
                if (dictionary.Key == obj.GetType().Name)
                {
                    foreach (string location in dictionary.Value)
                    {
                        string targetLocation = (location == "NewFolder"
                            ? obj.GetType().Name + "_" + Convert.ToInt16(obj.GetType().GetRuntimeProperty("Id").GetValue(obj))
                            : (containedInObject != null && location == "DynamicFolder"
                            ? containedInObject.GetType().Name + "_" + Convert.ToInt16(containedInObject.GetType().GetRuntimeProperty("Id").GetValue(containedInObject))
                            : location));
                        StorageFolder = await StorageFolder.CreateFolderAsync(targetLocation, CreationCollisionOption.OpenIfExists);
                    }
                }
            }
        }

        // Skriver XML fil - Dynamisk metode (Object)
        private async Task WriteFile(Object passedObject)
        {
            using (IRandomAccessStream stream = await TempStorageFile.OpenAsync(FileAccessMode.ReadWrite))
            using (Stream outputStream = stream.AsStreamForWrite())
            {
                DataContractSerializer serializer = new DataContractSerializer(passedObject.GetType());
                serializer.WriteObject(outputStream, passedObject);
            }
        }



        // Læser XML filer og genererer Liste af objekter.
        private async Task<ObservableCollection<Object>> ReadFiles(Type passedType, string foreignKey = null)
        {
            ObservableCollection<Object> thisList = new ObservableCollection<Object>();
            var TempStorageFiles = await TempStorageFolder.GetFilesAsync();
            if (TempStorageFiles.Count > 0)
            {
                foreach (var storageFile in TempStorageFiles)
                {
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    using (Stream inputStream = stream.AsStreamForRead())
                    {
                        DataContractSerializer serializer = new DataContractSerializer(passedType);
                        Object tempObject = serializer.ReadObject(inputStream) as Object;

                        if (foreignKey != null)
                        {
                            if (Convert.ToInt16(TempObject.GetType().GetRuntimeProperty("Id").GetValue(TempObject)) !=
                                Convert.ToInt16(tempObject.GetType().GetRuntimeProperty(foreignKey).GetValue(tempObject)))
                                break;
                        }
                        thisList.Add(tempObject);
                    }
                }
            }
            TempStorageFolder = StorageFolder;
            return thisList;
        }

        // Læser XML filer og genererer Liste af objekter.
        public async Task<ObservableCollection<Object>> XMLToList(Object passedObject, string foreignKey = "FK_Id")
        {
            // Sætter mappehieraki udfra objekt.
            await FolderLocation(passedObject, ReadWrite: true);
            var currentFolder = await StorageFolder.GetFoldersAsync();

            // Tjekker for eventuelle undermapper.
            if (currentFolder.Count > 0)
            {
                // Looper alle undermapper.
                foreach (var storageFolder in currentFolder)
                {
                    if ((await storageFolder.GetFilesAsync()).Count > 0)
                    {
                        // Åbner XML fil tilhørende objektet. -> Objekter med tilhørende lister, vil altid have undermapper i deres mapper
                        // Derfor henter vi først objekt xml'en nedenfor og looper igennem mapperne tilhørende objektet.
                        string[] ObjectName = storageFolder.Name.Split('_');
                        var storageFile =
                            await storageFolder.GetFileAsync(passedObject.GetType().Name + "_" + ObjectName[1] + ".xml");
                        using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                        using (Stream inputStream = stream.AsStreamForRead())
                        {
                            // Genererer Objekt af typen Objekt fra XML.
                            DataContractSerializer serializer = new DataContractSerializer(passedObject.GetType());
                            TempObject = serializer.ReadObject(inputStream) as Object;

                            await AddListToObject(foreignKey);
                            ObjectList.Add(TempObject);
                        }
                    }
                }
            }
            else
            {
                TempStorageFolder = StorageFolder;

                ObjectList = await ReadFiles(passedObject.GetType());
            }
            StorageFolder = KnownFolders.DocumentsLibrary;
            return ObjectList;
        }

        // Tilføjer objekter til liste af type objekt -> Bruges til at tilføje lister til ukendt objekt.
        private async Task AddListToObject(string foreignKey = "FK_Id")
        {
            // Looper objektet properties for at finde mærkede properties
            var PropList = TempObject.GetType().GetRuntimeProperties().ToList();
            foreach (PropertyInfo ShowProp in PropList)
            {
                // Hvis property er mærket med attribut<MarkedAttribute> -> Mærkede objekter (Lister) har ekstern xml data.
                if (ShowProp.GetCustomAttribute<MarkedAttribute>() != null)
                {
                    StorageFolder = KnownFolders.DocumentsLibrary;

                    // Læser property type.
                    Type objectType = ShowProp.PropertyType.GenericTypeArguments[0];
                    // Opretter en instans af propertien.
                    object objectInstance = Activator.CreateInstance(objectType);
                    // Sætter mappehieraki udfra objekt.
                    await FolderLocation(objectInstance, TempObject, true);
                    TempStorageFolder = StorageFolder;

                    // Opretter midlertidig ObservableCollection af typen fra objektet.
                    var listType = typeof(ObservableCollection<>);
                    var concreteType = listType.MakeGenericType(objectType);
                    var newList = Activator.CreateInstance(concreteType);
                    // Genererer objekter ud fra xmlfiler tilhørende objekt listen.
                    var objectList = await ReadFiles(objectType, foreignKey);
                    // Looper objekter læst.
                    foreach (var obj in objectList)
                        ((dynamic)newList).Add((dynamic)obj);


                    TempObject.GetType().GetRuntimeProperty(ShowProp.Name).SetValue(TempObject, newList);

                }
            }
        }

        public XMLReader()
        {
            // Definerer mappehieraki til objekttyper - NewFolder opretter ny dynamisk mappe - DynamicFolder henter dynamisk mappe
            FolderLocations = new Dictionary<string, List<string>>();
            FolderLocations.Add("Location", new List<string> { "Location" });
            FolderLocations.Add("Node", new List<string> { "Node" });
            FolderLocations.Add("Edge", new List<string> { "Edge" });

            FolderLocationsRead = new Dictionary<string, List<string>>();
            foreach (var items in FolderLocations)
                FolderLocationsRead.Add(items.Key, items.Value);
            
        }
    }
}
