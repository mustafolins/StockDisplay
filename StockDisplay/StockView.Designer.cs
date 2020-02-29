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
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.PredictionLabel = new System.Windows.Forms.Label();
            this.TommorowPredictionLabel = new System.Windows.Forms.Label();
            this.CurrentProgress = new System.Windows.Forms.ProgressBar();
            this.SymbolToLoad = new System.Windows.Forms.TextBox();
            this.Go = new System.Windows.Forms.Button();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchResults = new System.Windows.Forms.ComboBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchTextLabel = new System.Windows.Forms.Label();
            this.SearchResultsLabel = new System.Windows.Forms.Label();
            this.TomorrowsPredictionLabel2 = new System.Windows.Forms.Label();
            this.PredictionLabel2 = new System.Windows.Forms.Label();
            this.TomorrowsPredictionLabel3 = new System.Windows.Forms.Label();
            this.PredictionLabel3 = new System.Windows.Forms.Label();
            this.PatternLength = new System.Windows.Forms.Label();
            this.Pattern1LengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.Pattern2LenthUpDown = new System.Windows.Forms.NumericUpDown();
            this.Pattern2Length = new System.Windows.Forms.Label();
            this.Pattern3LengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.Pattern3Length = new System.Windows.Forms.Label();
            this.AveragePredictionLabel = new System.Windows.Forms.Label();
            this.ExpectedChangeLabel = new System.Windows.Forms.Label();
            this.SpxAverage = new System.Windows.Forms.Label();
            this.LodingInfoLabel = new System.Windows.Forms.Label();
            this.InclusiveAverage = new System.Windows.Forms.Label();
            this.ChartLength = new System.Windows.Forms.ComboBox();
            this.ChartLengthLabel = new System.Windows.Forms.Label();
            this.AccuracyLabel = new System.Windows.Forms.Label();
            this.GetAccuracyCheckBox = new System.Windows.Forms.CheckBox();
            this.AccuracyTestSize = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pattern1LengthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pattern2LenthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pattern3LengthUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 150);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 4;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Blue;
            series2.Legend = "Legend1";
            series2.Name = "MovingAverage";
            series2.ToolTip = "#VAL{C}";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.Red;
            series3.Legend = "Legend1";
            series3.Name = "MovingAverage30";
            series3.ToolTip = "#VAL{C}";
            series4.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.Fuchsia;
            series4.Legend = "Legend1";
            series4.Name = "BBUpper10";
            series5.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.Fuchsia;
            series5.Legend = "Legend1";
            series5.Name = "BBLower10";
            series6.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.Purple;
            series6.Legend = "Legend1";
            series6.Name = "BBUpper30";
            series7.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.Purple;
            series7.Legend = "Legend1";
            series7.Name = "BBLower30";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Series.Add(series5);
            this.chart1.Series.Add(series6);
            this.chart1.Series.Add(series7);
            this.chart1.Size = new System.Drawing.Size(835, 305);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
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
            // CurrentProgress
            // 
            this.CurrentProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentProgress.Location = new System.Drawing.Point(401, 475);
            this.CurrentProgress.Name = "CurrentProgress";
            this.CurrentProgress.Size = new System.Drawing.Size(100, 23);
            this.CurrentProgress.Step = 1;
            this.CurrentProgress.TabIndex = 5;
            this.CurrentProgress.Visible = false;
            // 
            // SymbolToLoad
            // 
            this.SymbolToLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SymbolToLoad.Location = new System.Drawing.Point(666, 475);
            this.SymbolToLoad.Name = "SymbolToLoad";
            this.SymbolToLoad.Size = new System.Drawing.Size(100, 20);
            this.SymbolToLoad.TabIndex = 7;
            this.SymbolToLoad.Text = "GOOG";
            // 
            // Go
            // 
            this.Go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Go.Location = new System.Drawing.Point(772, 475);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(75, 23);
            this.Go.TabIndex = 6;
            this.Go.Text = "Go";
            this.Go.UseVisualStyleBackColor = true;
            this.Go.Click += new System.EventHandler(this.Go_ClickAsync);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchTextBox.Location = new System.Drawing.Point(15, 478);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(100, 20);
            this.SearchTextBox.TabIndex = 8;
            this.SearchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // SearchResults
            // 
            this.SearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchResults.FormattingEnabled = true;
            this.SearchResults.Location = new System.Drawing.Point(121, 478);
            this.SearchResults.Name = "SearchResults";
            this.SearchResults.Size = new System.Drawing.Size(121, 21);
            this.SearchResults.TabIndex = 9;
            this.SearchResults.SelectedIndexChanged += new System.EventHandler(this.SearchResults_SelectedIndexChanged);
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchButton.Location = new System.Drawing.Point(248, 478);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 10;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchTextLabel
            // 
            this.SearchTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchTextLabel.AutoSize = true;
            this.SearchTextLabel.Location = new System.Drawing.Point(12, 462);
            this.SearchTextLabel.Name = "SearchTextLabel";
            this.SearchTextLabel.Size = new System.Drawing.Size(102, 13);
            this.SearchTextLabel.TabIndex = 11;
            this.SearchTextLabel.Text = "Search Text Symbol";
            // 
            // SearchResultsLabel
            // 
            this.SearchResultsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchResultsLabel.AutoSize = true;
            this.SearchResultsLabel.Location = new System.Drawing.Point(120, 462);
            this.SearchResultsLabel.Name = "SearchResultsLabel";
            this.SearchResultsLabel.Size = new System.Drawing.Size(79, 13);
            this.SearchResultsLabel.TabIndex = 12;
            this.SearchResultsLabel.Text = "Search Results";
            // 
            // TomorrowsPredictionLabel2
            // 
            this.TomorrowsPredictionLabel2.AutoSize = true;
            this.TomorrowsPredictionLabel2.Location = new System.Drawing.Point(12, 63);
            this.TomorrowsPredictionLabel2.Name = "TomorrowsPredictionLabel2";
            this.TomorrowsPredictionLabel2.Size = new System.Drawing.Size(109, 13);
            this.TomorrowsPredictionLabel2.TabIndex = 14;
            this.TomorrowsPredictionLabel2.Text = "Tomorrows Prediction";
            // 
            // PredictionLabel2
            // 
            this.PredictionLabel2.AutoSize = true;
            this.PredictionLabel2.Location = new System.Drawing.Point(12, 47);
            this.PredictionLabel2.Name = "PredictionLabel2";
            this.PredictionLabel2.Size = new System.Drawing.Size(100, 13);
            this.PredictionLabel2.TabIndex = 13;
            this.PredictionLabel2.Text = "Today\'s Prediction: ";
            // 
            // TomorrowsPredictionLabel3
            // 
            this.TomorrowsPredictionLabel3.AutoSize = true;
            this.TomorrowsPredictionLabel3.Location = new System.Drawing.Point(12, 101);
            this.TomorrowsPredictionLabel3.Name = "TomorrowsPredictionLabel3";
            this.TomorrowsPredictionLabel3.Size = new System.Drawing.Size(109, 13);
            this.TomorrowsPredictionLabel3.TabIndex = 16;
            this.TomorrowsPredictionLabel3.Text = "Tomorrows Prediction";
            // 
            // PredictionLabel3
            // 
            this.PredictionLabel3.AutoSize = true;
            this.PredictionLabel3.Location = new System.Drawing.Point(12, 85);
            this.PredictionLabel3.Name = "PredictionLabel3";
            this.PredictionLabel3.Size = new System.Drawing.Size(100, 13);
            this.PredictionLabel3.TabIndex = 15;
            this.PredictionLabel3.Text = "Today\'s Prediction: ";
            // 
            // PatternLength
            // 
            this.PatternLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PatternLength.AutoSize = true;
            this.PatternLength.Location = new System.Drawing.Point(761, 9);
            this.PatternLength.Name = "PatternLength";
            this.PatternLength.Size = new System.Drawing.Size(86, 13);
            this.PatternLength.TabIndex = 17;
            this.PatternLength.Text = "Pattern 1 Length";
            // 
            // Pattern1LengthUpDown
            // 
            this.Pattern1LengthUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pattern1LengthUpDown.Location = new System.Drawing.Point(764, 25);
            this.Pattern1LengthUpDown.Name = "Pattern1LengthUpDown";
            this.Pattern1LengthUpDown.Size = new System.Drawing.Size(83, 20);
            this.Pattern1LengthUpDown.TabIndex = 18;
            this.Pattern1LengthUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // Pattern2LenthUpDown
            // 
            this.Pattern2LenthUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pattern2LenthUpDown.Location = new System.Drawing.Point(764, 64);
            this.Pattern2LenthUpDown.Name = "Pattern2LenthUpDown";
            this.Pattern2LenthUpDown.Size = new System.Drawing.Size(83, 20);
            this.Pattern2LenthUpDown.TabIndex = 20;
            this.Pattern2LenthUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // Pattern2Length
            // 
            this.Pattern2Length.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pattern2Length.AutoSize = true;
            this.Pattern2Length.Location = new System.Drawing.Point(761, 48);
            this.Pattern2Length.Name = "Pattern2Length";
            this.Pattern2Length.Size = new System.Drawing.Size(86, 13);
            this.Pattern2Length.TabIndex = 19;
            this.Pattern2Length.Text = "Pattern 2 Length";
            // 
            // Pattern3LengthUpDown
            // 
            this.Pattern3LengthUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pattern3LengthUpDown.Location = new System.Drawing.Point(764, 103);
            this.Pattern3LengthUpDown.Name = "Pattern3LengthUpDown";
            this.Pattern3LengthUpDown.Size = new System.Drawing.Size(83, 20);
            this.Pattern3LengthUpDown.TabIndex = 22;
            this.Pattern3LengthUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // Pattern3Length
            // 
            this.Pattern3Length.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Pattern3Length.AutoSize = true;
            this.Pattern3Length.Location = new System.Drawing.Point(761, 87);
            this.Pattern3Length.Name = "Pattern3Length";
            this.Pattern3Length.Size = new System.Drawing.Size(86, 13);
            this.Pattern3Length.TabIndex = 21;
            this.Pattern3Length.Text = "Pattern 3 Length";
            // 
            // AveragePredictionLabel
            // 
            this.AveragePredictionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AveragePredictionLabel.AutoSize = true;
            this.AveragePredictionLabel.Location = new System.Drawing.Point(573, 9);
            this.AveragePredictionLabel.Name = "AveragePredictionLabel";
            this.AveragePredictionLabel.Size = new System.Drawing.Size(90, 13);
            this.AveragePredictionLabel.TabIndex = 23;
            this.AveragePredictionLabel.Text = "Average Percent:";
            // 
            // ExpectedChangeLabel
            // 
            this.ExpectedChangeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExpectedChangeLabel.AutoSize = true;
            this.ExpectedChangeLabel.Location = new System.Drawing.Point(573, 27);
            this.ExpectedChangeLabel.Name = "ExpectedChangeLabel";
            this.ExpectedChangeLabel.Size = new System.Drawing.Size(84, 13);
            this.ExpectedChangeLabel.TabIndex = 24;
            this.ExpectedChangeLabel.Text = "Expected Close:";
            // 
            // SpxAverage
            // 
            this.SpxAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SpxAverage.AutoSize = true;
            this.SpxAverage.Location = new System.Drawing.Point(573, 47);
            this.SpxAverage.Name = "SpxAverage";
            this.SpxAverage.Size = new System.Drawing.Size(71, 13);
            this.SpxAverage.TabIndex = 25;
            this.SpxAverage.Text = "SPX Percent:";
            // 
            // LodingInfoLabel
            // 
            this.LodingInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LodingInfoLabel.AutoSize = true;
            this.LodingInfoLabel.Location = new System.Drawing.Point(507, 481);
            this.LodingInfoLabel.Name = "LodingInfoLabel";
            this.LodingInfoLabel.Size = new System.Drawing.Size(0, 13);
            this.LodingInfoLabel.TabIndex = 26;
            // 
            // InclusiveAverage
            // 
            this.InclusiveAverage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InclusiveAverage.AutoSize = true;
            this.InclusiveAverage.Location = new System.Drawing.Point(573, 66);
            this.InclusiveAverage.Name = "InclusiveAverage";
            this.InclusiveAverage.Size = new System.Drawing.Size(126, 13);
            this.InclusiveAverage.TabIndex = 27;
            this.InclusiveAverage.Text = "Average (Including SPX):";
            // 
            // ChartLength
            // 
            this.ChartLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChartLength.FormattingEnabled = true;
            this.ChartLength.Items.AddRange(new object[] {
            "30",
            "90",
            "180",
            "365"});
            this.ChartLength.Location = new System.Drawing.Point(576, 101);
            this.ChartLength.Name = "ChartLength";
            this.ChartLength.Size = new System.Drawing.Size(121, 21);
            this.ChartLength.TabIndex = 28;
            // 
            // ChartLengthLabel
            // 
            this.ChartLengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChartLengthLabel.AutoSize = true;
            this.ChartLengthLabel.Location = new System.Drawing.Point(573, 85);
            this.ChartLengthLabel.Name = "ChartLengthLabel";
            this.ChartLengthLabel.Size = new System.Drawing.Size(71, 13);
            this.ChartLengthLabel.TabIndex = 29;
            this.ChartLengthLabel.Text = "Chart Length:";
            // 
            // AccuracyLabel
            // 
            this.AccuracyLabel.AutoSize = true;
            this.AccuracyLabel.Location = new System.Drawing.Point(259, 128);
            this.AccuracyLabel.Name = "AccuracyLabel";
            this.AccuracyLabel.Size = new System.Drawing.Size(55, 13);
            this.AccuracyLabel.TabIndex = 30;
            this.AccuracyLabel.Text = "Accuracy:";
            // 
            // GetAccuracyCheckBox
            // 
            this.GetAccuracyCheckBox.AutoSize = true;
            this.GetAccuracyCheckBox.Location = new System.Drawing.Point(156, 127);
            this.GetAccuracyCheckBox.Name = "GetAccuracyCheckBox";
            this.GetAccuracyCheckBox.Size = new System.Drawing.Size(91, 17);
            this.GetAccuracyCheckBox.TabIndex = 31;
            this.GetAccuracyCheckBox.Text = "Get Accuracy";
            this.GetAccuracyCheckBox.UseVisualStyleBackColor = true;
            this.GetAccuracyCheckBox.CheckedChanged += new System.EventHandler(this.GetAccuracyCheckBox_CheckedChanged);
            // 
            // AccuracyTestSize
            // 
            this.AccuracyTestSize.FormattingEnabled = true;
            this.AccuracyTestSize.Items.AddRange(new object[] {
            "4",
            "8",
            "16"});
            this.AccuracyTestSize.Location = new System.Drawing.Point(15, 123);
            this.AccuracyTestSize.Name = "AccuracyTestSize";
            this.AccuracyTestSize.Size = new System.Drawing.Size(135, 21);
            this.AccuracyTestSize.TabIndex = 32;
            this.AccuracyTestSize.Text = "Accuracy Test Size(%)";
            // 
            // StockView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 510);
            this.Controls.Add(this.AccuracyTestSize);
            this.Controls.Add(this.GetAccuracyCheckBox);
            this.Controls.Add(this.AccuracyLabel);
            this.Controls.Add(this.ChartLengthLabel);
            this.Controls.Add(this.ChartLength);
            this.Controls.Add(this.InclusiveAverage);
            this.Controls.Add(this.LodingInfoLabel);
            this.Controls.Add(this.SpxAverage);
            this.Controls.Add(this.ExpectedChangeLabel);
            this.Controls.Add(this.AveragePredictionLabel);
            this.Controls.Add(this.Pattern3LengthUpDown);
            this.Controls.Add(this.Pattern3Length);
            this.Controls.Add(this.Pattern2LenthUpDown);
            this.Controls.Add(this.Pattern2Length);
            this.Controls.Add(this.Pattern1LengthUpDown);
            this.Controls.Add(this.PatternLength);
            this.Controls.Add(this.TomorrowsPredictionLabel3);
            this.Controls.Add(this.PredictionLabel3);
            this.Controls.Add(this.TomorrowsPredictionLabel2);
            this.Controls.Add(this.PredictionLabel2);
            this.Controls.Add(this.SearchResultsLabel);
            this.Controls.Add(this.SearchTextLabel);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchResults);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.SymbolToLoad);
            this.Controls.Add(this.Go);
            this.Controls.Add(this.CurrentProgress);
            this.Controls.Add(this.TommorowPredictionLabel);
            this.Controls.Add(this.PredictionLabel);
            this.Controls.Add(this.chart1);
            this.Name = "StockView";
            this.Text = "Stock Display";
            this.Load += new System.EventHandler(this.StockView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pattern1LengthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pattern2LenthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pattern3LengthUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public System.Windows.Forms.Label PredictionLabel;
        public System.Windows.Forms.Label TommorowPredictionLabel;
        public System.Windows.Forms.ProgressBar CurrentProgress;
        private System.Windows.Forms.TextBox SymbolToLoad;
        private System.Windows.Forms.Button Go;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.ComboBox SearchResults;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Label SearchTextLabel;
        private System.Windows.Forms.Label SearchResultsLabel;
        public System.Windows.Forms.Label TomorrowsPredictionLabel2;
        public System.Windows.Forms.Label PredictionLabel2;
        public System.Windows.Forms.Label TomorrowsPredictionLabel3;
        public System.Windows.Forms.Label PredictionLabel3;
        private System.Windows.Forms.Label PatternLength;
        private System.Windows.Forms.NumericUpDown Pattern1LengthUpDown;
        private System.Windows.Forms.NumericUpDown Pattern2LenthUpDown;
        private System.Windows.Forms.Label Pattern2Length;
        private System.Windows.Forms.NumericUpDown Pattern3LengthUpDown;
        private System.Windows.Forms.Label Pattern3Length;
        private System.Windows.Forms.Label AveragePredictionLabel;
        private System.Windows.Forms.Label ExpectedChangeLabel;
        private System.Windows.Forms.Label SpxAverage;
        private System.Windows.Forms.Label LodingInfoLabel;
        private System.Windows.Forms.Label InclusiveAverage;
        private System.Windows.Forms.ComboBox ChartLength;
        private System.Windows.Forms.Label ChartLengthLabel;
        private System.Windows.Forms.Label AccuracyLabel;
        private System.Windows.Forms.CheckBox GetAccuracyCheckBox;
        private System.Windows.Forms.ComboBox AccuracyTestSize;
    }
}

