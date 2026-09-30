using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace homework_2
{
    public partial class MainWindow : Window
    {
        // 建立 Dictionary 來儲存飲料品項與對應的價格
        Dictionary<string, int> drinks = new Dictionary<string, int>()
        {
            { "招牌紅茶", 30 },
            { "珍珠奶茶", 50 }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            // 建立另一個 Dictionary 來儲存使用者的訂單內容 (品名, 數量)
            Dictionary<string, int> orders = new Dictionary<string, int>();

            int totalPrice = 0;
            string resultText = "【訂單明細】\n";

            // 1. 紀錄購買方式 (RadioButton)
            resultText += rbDineIn.IsChecked == true ? "用餐方式：內用\n" : "用餐方式：外帶\n";
            resultText += "------------------------\n";

            // 2. 動態取得使用者選取的訂單品項 (透過走訪 StackPanel 裡面的元件)
            foreach (UIElement element in DrinkMenuStackPanel.Children)
            {
                if (element is StackPanel sp)
                {
                    // 根據我們在 XAML 裡的排版，第 0 個是 CheckBox，第 2 個是 Slider
                    var cb = sp.Children[0] as CheckBox;
                    var sl = sp.Children[2] as Slider;

                    int quantity = (int)sl.Value;

                    // 如果有勾選，且滑桿數量大於 0
                    if (cb.IsChecked == true && quantity > 0)
                    {
                        string drinkName = cb.Content.ToString();
                        int price = drinks[drinkName]; // 從字典查價格

                        orders.Add(drinkName, quantity);
                        totalPrice += price * quantity;

                        resultText += $"{drinkName} x {quantity}杯 = {price * quantity}元\n";
                    }
                }
            }

            // 3. 防呆機制：如果什麼都沒買
            if (orders.Count == 0)
            {
                MessageBox.Show("請至少勾選一杯飲料並調整數量！", "提醒");
                return;
            }

            // 4. 加上售價折扣算法 (CheckBox)
            resultText += "------------------------\n";
            if (cbDiscount.IsChecked == true)
            {
                resultText += $"原價：{totalPrice} 元\n";
                totalPrice = (int)(totalPrice * 0.9); // 打 9 折
                resultText += $"折扣後總計：{totalPrice} 元\n";
            }
            else
            {
                resultText += $"總計：{totalPrice} 元\n";
            }

            // 將結果顯示在畫面下方
            txtResult.Text = resultText;
        }
    }
}