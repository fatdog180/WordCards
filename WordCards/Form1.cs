using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordCards
{
    public partial class frmWordCards : Form
    {
        // 1. 設定共用成員變數
        WordCollection _WordList = new WordCollection();                  // 單字清單集合
        WMPLib.WindowsMediaPlayer wmp = new WMPLib.WindowsMediaPlayer();  // Windows Media Player 播放器
        string strWordFile = "WordCards.txt";                             // 單字檔名
        bool isPlay = false;                                              // 是否處於自動播放狀態
        public frmWordCards()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 顯示單字：將傳入的單字物件內容填入對應的控制項
        /// </summary>
        private void ShowWord(WordItem word)
        {
            txtWord.Text = word.Word;             // 顯示單字
            txtPhonogram.Text = word.Phonogram;   // 顯示音標
            txtExplain.Text = word.Explain;       // 顯示解釋
        }

        /// <summary>
        /// 更新單字清單：將集合內的單字全部倒進 ListBox 中
        /// </summary>
        private void UpdateWordList()
        {
            lstWordList.BeginUpdate(); // 開始更新（暫停重繪，避免大量資料倒進去時畫面閃爍）
            lstWordList.Items.Clear(); // 清空目前的項目

            foreach (WordItem item in this._WordList)
            {
                lstWordList.Items.Add(item); // 將單字項目加入 ListBox
            }

            lstWordList.EndUpdate(); // 結束更新（恢復重繪）
        }

        /// <summary>
        /// 播放單字音檔
        /// </summary>
        public void PlayWord(WordItem word)
        {
            // 檢查音效檔是否存在 [cite: 1134]
            if (File.Exists(word.SoundPath))
            {
                wmp.URL = word.SoundPath;          // 設定播放路徑
                wmp.settings.autoStart = true;     // 設定自動播放
                wmp.settings.mute = false;         // 確保沒有靜音
                wmp.controls.play();               // 開始播放
            }
            else
            {
                // 若找不到音檔，在狀態列提示
                tsslMessage.Text = $"找無 {word.SoundPath} 音效檔";
            }
        }

        /// <summary>
        /// 讀取並播放目前選取的單字
        /// </summary>
        private void PlaySelectedWord()
        {
            // 判斷目前選取的項目是否為空
            if (lstWordList.SelectedItem != null)
            {
                // 取得目前選取的單字索引
                int idx = lstWordList.SelectedIndex;

                // 顯示單字內容
                ShowWord(_WordList[idx]);

                // 播放該單字的發音
                PlayWord(_WordList[idx]);
            }
        }

        /// <summary>
        /// 將單字清單的選項移到下一個
        /// </summary>
        private void NextWordList()
        {
            lstWordList.Focus(); // 將焦點移到單字清單

            // 判斷目前選的下一項是否超過清單的項目數
            if (lstWordList.SelectedIndex + 1 >= lstWordList.Items.Count)
            {
                lstWordList.SelectedIndex = 0; // 如果超過就回到第一項
            }
            else
            {
                lstWordList.SelectedIndex++; // 如果沒有就選擇下一項
            }

            // 計算目前 lstWordList 畫面真正顯示的行數
            int lstRows = lstWordList.Height / lstWordList.GetItemHeight(0);

            // 如果目前選的項目大於顯示行數的一半，就自動把選項保持在正中間
            if (lstWordList.SelectedIndex >= lstRows / 2)
            {
                lstWordList.TopIndex = lstWordList.SelectedIndex - (lstRows / 2);
            }
        }

        private void lstWordList_Click(object sender, EventArgs e)
        {
            // 如果目前處於「自動播放」狀態，點擊其他單字時就先關閉自動播放
            if (isPlay == true)
            {
                btnAutoPlay.PerformClick(); // 模擬點擊自動播放按鈕來停止它
            }

            // 判斷是否有選取項目
            if (lstWordList.SelectedItem != null)
            {
                // 顯示並播放目前選取的單字
                PlaySelectedWord();
            }
        }

        private void frmWordCards_Load(object sender, EventArgs e)
        {
            string[] lines;

            // 檢查單字檔是否存在
            if (File.Exists(strWordFile))
            {
                // 讀取檔案，並指定 UTF8 編碼以防音標變亂碼
                lines = File.ReadAllLines(strWordFile, Encoding.UTF8);
            }
            else
            {
                // 若找不到檔案，跳出錯誤提示並關閉程式
                MessageBox.Show($"找不到單字檔\n{strWordFile}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // 載入單字檔字串陣列至集合中
            _WordList.LoadFromStringArray(lines);

            // 如果有成功載入到單字
            if (this._WordList.Count > 0)
            {
                UpdateWordList();           // 更新左側單字清單
                ShowWord(_WordList[0]);     // 預先顯示第一個單字
                tsslMessage.Text = $"單字數量:{_WordList.Count}"; // 在下方狀態列顯示數量
            }
        }

        // 點擊自動播放按鈕
        private void btnAutoPlay_Click(object sender, EventArgs e)
        {
            lstWordList.Focus(); // 將焦點移到單字清單

            if (isPlay == false) // 若目前不是自動播放
            {
                btnAutoPlay.Text = "Stop"; // 按鈕文字變成 Stop
                isPlay = true;             // 修改狀態為正在播放
                PlaySelectedWord();        // 顯示並播放目前選取的單字
                timPlayer.Start();         // 啟動計時器開始巡航
            }
            else
            {
                btnAutoPlay.Text = "Play"; // 按鈕文字變回 Play
                isPlay = false;            // 修改狀態為停止
                timPlayer.Stop();          // 停止計時器
            }
        }

        // 當計時器時間到（每 2 秒）觸發的事件
        private void timPlayer_Tick(object sender, EventArgs e)
        {
            NextWordList();       // 移到下一個單字
            PlaySelectedWord();   // 顯示並播放目前選取的單字
        }

        // 當使用者按下鍵盤按鍵時
        private void frmWordCards_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isPlay == true) return; // 如果處於自動播放狀態，就不觸發手動快捷鍵

            switch (e.KeyChar) // 判斷按下的字元
            {
                case (char)Keys.Return: // 當按下 Enter 時
                    NextWordList();     // 切換到下一個單字
                    PlaySelectedWord(); // 顯示並播放單字
                    e.Handled = true;   // 代表事件已處理完畢，防止系統發出嗶聲
                    break;

                case (char)Keys.Space:  // 當按下 Space 空白鍵時
                    if (lstWordList.SelectedIndex >= 0) // 確保有選取項目
                    {
                        PlaySelectedWord(); // 重複播放目前單字
                        e.Handled = true;   // 代表事件已處理完畢
                    }
                    break;
            }
        }

        private void lstWordList_DoubleClick(object sender, EventArgs e)
        {
            lstWordList.Focus(); // 將焦點移回單字清單

            // 確保真的有選取到單字項目，且索引值大於等於 0
            if (lstWordList.SelectedIndex >= 0)
            {
                // 取得目前滑鼠雙擊的單字索引
                int idx = lstWordList.SelectedIndex;

                // 實體化編輯表單，並將目前選中的單字物件投遞過去
                frmEditWord edit = new frmEditWord(_WordList[idx]);

                // 以對話方塊（Modal）形式打開視窗，並等待使用者操作完畢
                DialogResult result = edit.ShowDialog(this);

                // 如果使用者在編輯視窗中按下「儲存按鈕」（即回傳 DialogResult.Yes）
                if (result == DialogResult.Yes)
                {
                    // 1. 重新刷新畫面中央的顯示內容，並播放發音
                    PlaySelectedWord();

                    // 2. 呼叫 WordCollection 內寫好的 SaveToFile 函式，直接將更新後的資料即時寫回檔案
                    _WordList.SaveToFile(strWordFile);

                    // 3. 在下方狀態列提示存檔成功
                    tsslMessage.Text = $"單字 [{_WordList[idx].Word}] 修改並儲存成功！";
                }
            }
        }
    }
}
