//using EnvDTE;
//using EnvDTE80;
//using System.Collections.Generic;

//namespace OpenInApp
//{
//    public class ArtefactsToOpenHelper
//    {
//        public static IList<string> GetArtefactsToBeOpened(DTE2 dte)
//        {
//            var result = new List<string>();

//            foreach (SelectedItem selectedItem in dte.SelectedItems)
//            {
//                var itemName = selectedItem.ProjectItem.FileNames[0];
//                result.Add(itemName);
//            }
            
//            return result;
//        }
//    }
//}
