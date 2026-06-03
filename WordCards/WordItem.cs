using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCards
{
    public class WordItem
    {
        // 定義單字的各項屬性
        public string Word { get; set; } = null;       // 單字
        public string Phonogram { get; set; } = null;  // 音標
        public string SoundPath { get; set; } = null;  // 音檔路徑
        public string Explain { get; set; } = null;    // 解釋

        /// <summary>
        /// 建構子：傳入單行的單字字串並自動解析
        /// </summary>
        public WordItem(string str)
        {
            // 使用 Tab 鍵 (\t) 分隔字串 [cite: 708]
            string[] strLists = str.Split('\t');
            if (strLists.Length >= 3)
            {
                Word = strLists[0];       // 第 1 欄為單字
                Phonogram = strLists[1];  // 第 2 欄為音標
                SoundPath = strLists[2];  // 第 3 欄為音檔路徑

                // 第 4 欄之後的所有內容合併為解釋（處理包含換行的狀況）
                Explain = string.Join(Environment.NewLine, strLists.Skip(3));
            }
        }

        /// <summary>
        /// 覆寫 ToString()，讓 ListBox 或其他控制項可以直接顯示單字名稱
        /// </summary>
        public override string ToString()
        {
            return Word;
        }

        /// <summary>
        /// 將 WordItem 物件還原成寫入檔案的 TSV 格式字串
        /// </summary>
        public string ToLineString()
        {
            // 先將解釋屬性中的換行符號替換回 \t，避免破壞單行格式
            string strExplain = Explain.Replace(Environment.NewLine, "\t");
            return $"{Word}\t{Phonogram}\t{SoundPath}\t{strExplain}";
        }
    }
}
