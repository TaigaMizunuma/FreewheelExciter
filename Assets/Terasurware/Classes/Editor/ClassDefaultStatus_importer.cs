using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ClassDefaultStatus_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/ClassDefaultStatus.xlsx";
    private static readonly string[] sheetNames = { "ClassDefaultStatus", };
    
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
                    var data = (Entity_ClassDefaultStatus)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_ClassDefaultStatus));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_ClassDefaultStatus>();
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
                        
                        var p = new Entity_ClassDefaultStatus.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.hp = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.str = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.skl = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.spd = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.luk = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.def = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.cur = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.move = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.hp_r = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.str_r = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.skl_r = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.spd_r = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.luk_r = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.def_r = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.cur_r = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.move_r = (int)(cell == null ? 0 : cell.NumericCellValue);

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
