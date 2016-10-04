using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Docker.DotNet.Reactive.Tests
{
	/// <summary>
    ///		Test suite for <see cref="NetworkedStreams"/>.
    /// </summary>
	/// <remarks>
    ///		<see cref="NetworkedStreams"/> is used by other tests to simulate streaming of events from the Docker API.
	/// 
	/// 	Consider this test suite to be a sanity check.
    /// </remarks>
    public class NetworkedStreamsTests
    {
		/// <summary>
        ///		Create a new networked streams test suite. 
        /// </summary>
        /// <param name="output"></param>
		public NetworkedStreamsTests(ITestOutputHelper output)
		{
			if (output == null)
				throw new ArgumentNullException(nameof(output));

			Output = output;
		}

		/// <summary>
        /// 	Output for the current test.
        /// </summary>
		ITestOutputHelper Output { get; }

		/// <summary>
        ///		Verify that we can write a line of text to <see cref="NetworkedStreams"/>, then read it back out again. 
        /// </summary>
        [Fact]
        public void CanReadAndWriteLine() 
        {
			SynchronizationContext.SetSynchronizationContext(
				new SynchronizationContext()
			);

			const string expectedText = "Hello, world!";
			
			string actualText = null;
			using (NetworkedStreams streams = new NetworkedStreams())
			{
				streams.Open();

				Output.WriteLine("Waiting for read and write tasks...");
				Task.WaitAll(
					tasks: new[]
					{
						Task.Factory.StartNew(() =>
						{
							Output.WriteLine("Reading...");
							Output.WriteLine(
								actualText = streams.Reader.ReadLine()
							);
							Output.WriteLine("Read.");
						}),
						Task.Factory.StartNew(() =>
						{
							Output.WriteLine("Writing...");
							streams.Writer.WriteLine(expectedText);
							Output.WriteLine("Wrote.");
						})
					},
					timeout: TimeSpan.FromSeconds(10)
				);
				Output.WriteLine("Read and write tasks completed.");
			}
			
            Assert.Equal(expectedText, actualText);
        }
    }
}
