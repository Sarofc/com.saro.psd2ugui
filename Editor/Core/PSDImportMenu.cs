using System;
using UnityEditor;
using UnityEngine;

namespace PSDUIImporter
{
    //------------------------------------------------------------------------------
    // class definition
    //------------------------------------------------------------------------------
    public class PSDImportMenu : ScriptableWizard
    {
        public TextAsset psdXml;

        [MenuItem("PSD2UGUI/Create Config", false, 2)]
        private static void CreateConfig()
        {
            var path = "Assets/Editor/PSD2UGUI/";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            PSD2UGUIConfig.instance.hideFlags = HideFlags.None;
            AssetDatabase.CreateAsset(PSD2UGUIConfig.instance, path + "PSD2UGUIConfig.asset");
        }

        [MenuItem("PSD2UGUI/Import XML ...", false, 1)]
        private static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<PSDImportMenu>("PSD 2 UGUI", "Cancel", "Import");
        }

        private void OnWizardUpdate()
        {
            helpString = "select psdxml file";
        }

        // import
        private void OnWizardOtherButton()
        {
            if (!psdXml)
            {
                return;
            }

            var inputFile = AssetDatabase.GetAssetPath(psdXml);

            if (!string.IsNullOrEmpty(inputFile))
            {
                PSDImportCtrl import = new PSDUIImporter.PSDImportCtrl(inputFile);
                import.BeginDrawUILayers();
                import.BeginSetUIParents();
            }

            GC.Collect();
        }

        // cancel
        private void OnWizardCreate()
        {

        }
    }
}