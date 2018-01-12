using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ClassList_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/ClassList.xlsx";
    private static readonly string[] sheetNames = { "ClassList", };
    
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
                    var exportPath = "Assets/Resources/Data/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Entity_ClassList)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_ClassList));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_ClassList>();
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
                        
                        var p = new Entity_ClassList.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.hp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.sp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.str = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.skl = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.spd = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.luk = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.def = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.cur = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.move = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.gun = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.rifle = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.knife = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.fist = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.spear = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.axe = (int)(cell == null ? 0 : cell.NumericCellValue);

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
