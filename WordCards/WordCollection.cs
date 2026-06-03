using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCards
{
    public class WordCollection : Collection<WordItem> // 繼承 Collection 集合 
    {
        /// <summary>
        /// 從字串陣列載入單字資料 [cite: 728]
        /// </summary>
        /// <param name="lines">輸入的單字字串陣列 </param>
        public void LoadFromStringArray(string[] lines)
        {
            this.Clear(); // 先清空目前集合內現有的資料 

            // 將字串陣列的資料逐行載入到集合中
            foreach (string line in lines)
            {
                // 略過空白行，避免解析時出錯
                if (string.IsNullOrWhiteSpace(line)) continue;

                // 產生 WordItem 物件並將其加入到集合中
                WordItem item = new WordItem(line);
                this.Add(item);
            }
        }

        /// <summary>
        /// 將 WordCollection 物件的資料儲存到檔案中
        /// </summary>
        /// <param name="filePath">要寫入的檔案路徑</param>
        public void SaveToFile(string filePath)
        {
            // 使用 StreamWriter 將資料儲存到檔案中
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (WordItem item in this)
                {
                    // 將每個單字項目轉換回 TSV 字串並寫入檔案
                    writer.WriteLine(item.ToLineString());
                }
            }
        }
    }
}
