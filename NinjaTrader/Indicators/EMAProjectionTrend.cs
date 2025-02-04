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
	public class EMAProjectionTrend : Indicator
	{
		private EMAProjection emaProjection;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "EMA Projection Trend";
				Calculate									= Calculate.OnPriceChange;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= false;
				DrawHorizontalGridLines						= false;
				DrawVerticalGridLines						= false;
				PaintPriceMarkers							= false;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				Period										= 14;

				AddPlot(new Stroke(Brushes.Green, 2), PlotStyle.Bar, "Trend Up");
				AddPlot(new Stroke(Brushes.Red, 2), PlotStyle.Bar, "Trend Down");
				Plots[0].AutoWidth = true;
				Plots[1].AutoWidth = true;
			}
			else if (State == State.DataLoaded)
			{				
				emaProjection = EMAProjection(false, Period);
			}
		}

		protected override void OnBarUpdate()
		{
			if (CurrentBar == 0) {
				return;
			}
			Series<double> projection = emaProjection.Projection;
			double trend = projection[0] - projection[1];
			TrendUp[0] = trend > 0 ? trend : 0;
			TrendDown[0] = trend < 0 ? trend : 0;
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="Period", Order=2, GroupName="Parameters")]
		public int Period
		{ get; set; }

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> TrendUp
		{
			get { return Values[0]; }
		}
		
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> TrendDown
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
		private EMAProjectionTrend[] cacheEMAProjectionTrend;
		public EMAProjectionTrend EMAProjectionTrend(int period)
		{
			return EMAProjectionTrend(Input, period);
		}

		public EMAProjectionTrend EMAProjectionTrend(ISeries<double> input, int period)
		{
			if (cacheEMAProjectionTrend != null)
				for (int idx = 0; idx < cacheEMAProjectionTrend.Length; idx++)
					if (cacheEMAProjectionTrend[idx] != null && cacheEMAProjectionTrend[idx].Period == period && cacheEMAProjectionTrend[idx].EqualsInput(input))
						return cacheEMAProjectionTrend[idx];
			return CacheIndicator<EMAProjectionTrend>(new EMAProjectionTrend(){ Period = period }, input, ref cacheEMAProjectionTrend);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.EMAProjectionTrend EMAProjectionTrend(int period)
		{
			return indicator.EMAProjectionTrend(Input, period);
		}

		public Indicators.EMAProjectionTrend EMAProjectionTrend(ISeries<double> input , int period)
		{
			return indicator.EMAProjectionTrend(input, period);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.EMAProjectionTrend EMAProjectionTrend(int period)
		{
			return indicator.EMAProjectionTrend(Input, period);
		}

		public Indicators.EMAProjectionTrend EMAProjectionTrend(ISeries<double> input , int period)
		{
			return indicator.EMAProjectionTrend(input, period);
		}
	}
}

#endregion
