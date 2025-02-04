#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class EMAProjection : Indicator
	{
		private EMA ema;
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Computes the EMA with an additional factor of prediction based on the slope of the T-1 and T-2 prices.";
				Name										= "EMA Projection";
				Calculate									= Calculate.OnPriceChange;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				Period										= 14;
				PrintOutput									= false;

				AddPlot(Brushes.Goldenrod, "EMA");
				AddPlot(Brushes.Aquamarine, "Projection");
			}
			else if (State == State.Configure)
			{
				ema = EMA(Period);
			}
		}

		protected override void OnBarUpdate()
		{
			double ema0 = ema[0];
			EMA[0] = ema0;
			
			// Since we are looking at our past EMA, we need at least N+1 periods so we have enough
			// data to look over ema[1] to ema[N].
			if (CurrentBar < Period + 1) {
				return;
			}
			
			double ema1 = ema[1];
			double emaX = ema[Period + 1];
			double dx = ema1 - emaX;
			int dy = Period - 1;
			double slope = dx / dy;
			Projection[0] = ema1 + slope;
			
			#region Debugging
			if (PrintOutput) {
				Print(string.Format(
					"{{" +
						"\"CurrentBar\": {0}, " +
						"\"ema{1}\": {2:0.00}, " +
						"\"ema1\": {3:0.00}, " +
						"\"period\": {4}, " +
						"\"slope\": {5:0.00}, " +
						"\"projection\": {6:0.00}" +
					"}}",
					CurrentBar, Period + 1, emaX, ema1, Period, slope, Projection[0]
				));
			}
			#endregion
		}

		#region Properties
		[NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Print Output", GroupName = "NinjaScriptParameters", Order = 0)]
		public bool PrintOutput { get; set; }
		
		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Period", GroupName = "NinjaScriptParameters", Order = 0)]
		public int Period { get; set; }
		
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> EMA
		{
			get { return Values[0]; }
		}
		
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> Projection
		{
			get { return Values[1]; }
		}
		#endregion
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private EMAProjection[] cacheEMAProjection;
		public EMAProjection EMAProjection(bool printOutput, int period)
		{
			return EMAProjection(Input, printOutput, period);
		}

		public EMAProjection EMAProjection(ISeries<double> input, bool printOutput, int period)
		{
			if (cacheEMAProjection != null)
				for (int idx = 0; idx < cacheEMAProjection.Length; idx++)
					if (cacheEMAProjection[idx] != null && cacheEMAProjection[idx].PrintOutput == printOutput && cacheEMAProjection[idx].Period == period && cacheEMAProjection[idx].EqualsInput(input))
						return cacheEMAProjection[idx];
			return CacheIndicator<EMAProjection>(new EMAProjection(){ PrintOutput = printOutput, Period = period }, input, ref cacheEMAProjection);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.EMAProjection EMAProjection(bool printOutput, int period)
		{
			return indicator.EMAProjection(Input, printOutput, period);
		}

		public Indicators.EMAProjection EMAProjection(ISeries<double> input , bool printOutput, int period)
		{
			return indicator.EMAProjection(input, printOutput, period);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.EMAProjection EMAProjection(bool printOutput, int period)
		{
			return indicator.EMAProjection(Input, printOutput, period);
		}

		public Indicators.EMAProjection EMAProjection(ISeries<double> input , bool printOutput, int period)
		{
			return indicator.EMAProjection(input, printOutput, period);
		}
	}
}

#endregion
