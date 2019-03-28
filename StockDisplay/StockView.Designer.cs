namespace StockDisplay
{
    partial class StockView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Go = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SymbolToLoad = new System.Windows.Forms.TextBox();
            this.PredictionLabel = new System.Windows.Forms.Label();
            this.TommorowPredictionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // Go
            // 
            this.Go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Go.Location = new System.Drawing.Point(713, 12);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(75, 23);
            this.Go.TabIndex = 0;
            this.Go.Text = "Go";
            this.Go.UseVisualStyleBackColor = true;
            this.Go.Click += new System.EventHandler(this.Go_Click);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 41);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 4;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(776, 397);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // SymbolToLoad
            // 
            this.SymbolToLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SymbolToLoad.Location = new System.Drawing.Point(607, 12);
            this.SymbolToLoad.Name = "SymbolToLoad";
            this.SymbolToLoad.Size = new System.Drawing.Size(100, 20);
            this.SymbolToLoad.TabIndex = 2;
            this.SymbolToLoad.Text = "GOOG";
            // 
            // PredictionLabel
            // 
            this.PredictionLabel.AutoSize = true;
            this.PredictionLabel.Location = new System.Drawing.Point(12, 9);
            this.PredictionLabel.Name = "PredictionLabel";
            this.PredictionLabel.Size = new System.Drawing.Size(100, 13);
            this.PredictionLabel.TabIndex = 3;
            this.PredictionLabel.Text = "Today\'s Prediction: ";
            // 
            // TommorowPredictionLabel
            // 
            this.TommorowPredictionLabel.AutoSize = true;
            this.TommorowPredictionLabel.Location = new System.Drawing.Point(12, 25);
            this.TommorowPredictionLabel.Name = "TommorowPredictionLabel";
            this.TommorowPredictionLabel.Size = new System.Drawing.Size(109, 13);
            this.TommorowPredictionLabel.TabIndex = 4;
            this.TommorowPredictionLabel.Text = "Tomorrows Prediction";
            // 
            // StockView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TommorowPredictionLabel);
            this.Controls.Add(this.PredictionLabel);
            this.Controls.Add(this.SymbolToLoad);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.Go);
            this.Name = "StockView";
            this.Text = "Stock Display";
            this.Load += new System.EventHandler(this.StockView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Go;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox SymbolToLoad;
        private System.Windows.Forms.Label PredictionLabel;
        private System.Windows.Forms.Label TommorowPredictionLabel;
    }
}

