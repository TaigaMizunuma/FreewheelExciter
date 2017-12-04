using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class StoryTestTwo_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Saito/Resources/Data/StoryTestTwo.xlsx";
    private static readonly string[] sheetNames = { "StoryTest", };
    
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/Saito/Resources/Data/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Entity_StoryTestTwo)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_StoryTestTwo));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_StoryTestTwo>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

					// check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                	// add infomation
                    for (int i=1; i<= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;
                        
                        var p = new Entity_StoryTestTwo.Param();
			
					cell = row.GetCell(0); p.StoryNumber = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.Story = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.LeftOneImage = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.RightOneImage = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.LeftTwoImage = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.RightTwoImage = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.LeftOne = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.RightOne = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.LeftTwo = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.RightTwo = (cell == null ? "" : cell.StringCellValue);

                        data.param.Add(p);
                    }
                    
                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }
}
