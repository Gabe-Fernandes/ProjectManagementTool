namespace PMT.Services.ProjectMetrics;

public interface IRazorToJsHub
{
  Task ReceivePieChartData(PieChartData pieChartData);
  Task ReceiveBarGraphData(BarGraphData barGraphData);
  Task ReceiveBurnDownChartData(BurnDownChartData burnDownChartData);
}
