using System;
using System.Text;
using System.Threading;

public class ProgressBar : IDisposable, IProgress<double>
{
	private int _blockCount;
	private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
	private const string animation = @"|/-\";

	private readonly Timer timer;

	private double currentProgress = 0;
	private string currentText = string.Empty;
	private bool disposed = false;
	private int animationIndex = 0;

	public ProgressBar(int blockCount = 50)
	{
		_blockCount = blockCount;
		timer = new Timer(TimerHandler);
		if (!Console.IsOutputRedirected) { ResetTimer(); }
	}

	public void Report(double value)
	{
		value = Math.Max(0, Math.Min(1, value));
		Interlocked.Exchange(ref currentProgress, value);
	}

	private void TimerHandler(object state)
	{
		lock (timer)
		{
			if (disposed) return;

			var progressBlockCount = (int)(currentProgress * _blockCount);
			var percent = (int)(currentProgress * 100);
			var text = string.Format("[{0}{1}] {2,3}% {3}",
				new string('#', progressBlockCount), new string('-', _blockCount - progressBlockCount),
				percent,
				animation[animationIndex++ % animation.Length]);
			UpdateText(text);

			ResetTimer();
		}
	}

	private void UpdateText(string text)
	{
		var commonPrefixLength = 0;
		var commonLength = Math.Min(currentText.Length, text.Length);
		while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
		{
			commonPrefixLength++;
		}

		var outputBuilder = new StringBuilder();
		outputBuilder.Append('\b', currentText.Length - commonPrefixLength);
		outputBuilder.Append(text.Substring(commonPrefixLength));

		var overlapCount = currentText.Length - text.Length;
		if (overlapCount > 0)
		{
			outputBuilder.Append(' ', overlapCount);
			outputBuilder.Append('\b', overlapCount);
		}

		Console.Write(outputBuilder);
		currentText = text;
	}

	private void ResetTimer()
	{
		timer.Change(animationInterval, TimeSpan.FromMilliseconds(-1));
	}

	public void Dispose()
	{
		lock (timer)
		{
			disposed = true;
			UpdateText(string.Empty);
		}
	}
}