# 視窗程式設計 (II) - 單字卡程式 (WordCards)

本專案為 C# Windows Forms 視窗程式設計課程之「單字卡程式」上課練習作業。程式整合了 TSV 資料檔讀取、自訂類別集合物件管理、Windows Media Player 音訊播放、Timer 自動巡航播放、表單鍵盤事件攔截（KeyPreview），以及雙擊項目彈出視窗修改資料並即時覆寫檔案等多項核心技術。

## 項目功能正確性說明

* **資料檔自動讀取**：程式啟動時會自動以 UTF-8 編碼讀取 `WordCards.txt` 檔案，並透過自訂建構子將 Tab 鍵分隔的各欄位解析為單字物件。
* **左側單字清單**：覆寫 `ToString()` 讓 ListBox 直接呈現單字名稱，並使用 `BeginUpdate` 與 `EndUpdate` 優化大量資料載入時的渲染效能。
* **多媒體發音播放**：整合 COM 元件 `Windows Media Player`，在點擊單字時自動偵測對應路徑之 `.mp3` 音檔並即時播放發音。
* **自動巡航功能**：點擊 「Play/Stop」按鈕可控制 `Timer` 計時器（間隔 2000ms），自動依序切換下一個單字並發音，且選取項目會動態保持在清單畫面正中央。
* **手動鍵盤快捷鍵**：開啟表單 `KeyPreview` 屬性，在非自動播放狀態下，支援敲擊 `Enter` 切換至下一個單字、敲擊 `Space`（空白鍵）重複目前單字發音。
* **雙擊編輯與即時存檔**：在清單項目上「雙擊滑鼠」可彈出 `frmEditWord` 對話方塊。使用者修改單字、音標、音檔路徑或解釋後點擊儲存，主畫面會同步重新整理，並即時覆寫更新回 `WordCards.txt` 檔案中。

## 執行說明

### 1. 開啟專案與環境設定
* 使用 Visual Studio 開啟 `WordCards.sln` 專案檔。
* 確保專案已成功加入 `Windows Media Player`（COM 元件 `WMPLib`）的參考。

### 2. 檔案屬性確認
為了確保程式執行時順利存取檔案，請確認以下檔案屬性設定：
* `WordCards.txt`：【組建動作】設定為 **內容** (Content)；【複製到輸出目錄】設定為 **有更新時才複製** (Copy if newer)。
* `Sound/A/` 資料夾下的所有 `.mp3` 檔案：【複製到輸出目錄】設定為 **有更新時才複製** (Copy if newer)。

### 3. 編譯與執行
* 按下 `F5` 或點選「啟動」按鈕執行程式。
* **手動模式**：點選左側清單切換單字，或使用 `Enter` / `Space` 鍵操作。
* **自動模式**：點選右側 `Play` 按鈕開啟每 2 秒自動播放，點選 `Stop` 停止。
* **修改模式**：雙擊左側清單中想修改的單字，即可在彈出的視窗中修改並儲存。

## 畫面截圖
<img width="728" height="305" alt="example1" src="https://github.com/user-attachments/assets/00aadf17-ab61-4b8b-8efe-fa49ae9975d8" />
<img width="231" height="397" alt="example2" src="https://github.com/user-attachments/assets/646a749c-7724-4a3e-8e51-7769058d19ff" />
