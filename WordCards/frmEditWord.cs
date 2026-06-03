using System;
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
    public partial class frmEditWord : Form
    {
        // 定義一個公開屬性，用來接收或傳遞單字物件
        public WordItem Word { get; set; } = null;

        // 修改建構子，讓主畫面在叫起這個表單時，可以直接把「選中的單字物件」丟進來
        public frmEditWord(WordItem word)
        {
            InitializeComponent();

            this.Word = word; // 暫存傳入的單字物件

            // 將原本的單字資料自動填入對應的輸入框中，方便使用者修改
            txtWord.Text = word.Word;
            txtPhonogram.Text = word.Phonogram;
            txtSoundPath.Text = word.SoundPath;
            txtExplain.Text = word.Explain;
        }

        // 儲存按鈕的點擊事件（請先去設計畫面雙擊 btnSave 按鈕來產生此事件綁定）
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 當使用者點擊儲存，就把輸入框內修改過的新文字，倒回去原本的物件屬性裡
            Word.Word = txtWord.Text;
            Word.Phonogram = txtPhonogram.Text;
            Word.SoundPath = txtSoundPath.Text;
            Word.Explain = txtExplain.Text;

            // 設定視窗關閉的回傳結果為 Yes，代表使用者是按儲存，而不是按右上角 X 關閉
            this.DialogResult = DialogResult.Yes;
            this.Close(); // 關閉此編輯視窗
        }
    }
}
