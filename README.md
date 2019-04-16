# StockDisplay
**Features:**
- Chart that shows the requested stock symbol data in candlestick style with the requested duration.
- Symbol search to help ease finding a companies stock symbol.
- AI analysis/prediction based on three different pattern lengths.
- Accuracy calculation for the AI (still in progress).

**AI:**

The AI uses a regression random forest to ML algorithm.  The data used with the AI are:
- Day(s) data
  - A few percentages pertaining to Open, High, Low, Close.
  - Volume.
  - Technical Indicators
    - Simple moving averages.
    - Standard deviation.
    - Bollinger bands.
- The percent growth of the next day.

*Disclaimer:* The AI is not intended as investment advice, invest at your own risk.