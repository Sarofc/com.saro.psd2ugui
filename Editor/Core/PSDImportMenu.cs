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
            var directory = System.IO.Path.GetDirectoryName(PSD2UGUIConfig.k_ConfigPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            var instance = ScriptableObject.CreateInstance<PSD2UGUIConfig>();
            AssetDatabase.CreateAsset(instance, PSD2UGUIConfig.k_ConfigPath);
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