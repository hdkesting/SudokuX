using Microsoft.VisualStudio.DebuggerVisualizers;
using System;

namespace SudokuX.Solver.Visualizers
{
    // TODO: Add the following to SomeType's definition to see this visualizer when debugging instances of SomeType:
    // 
    //  [DebuggerVisualizer(typeof(GridVisualizer))]
    //  [Serializable]
    //  public class SomeType
    //  {
    //   ...
    //  }
    // 
    /// <summary>
    /// A Visualizer for SomeType.  
    /// </summary>
    public class GridVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// Shows the specified grid.
        /// </summary>
        /// <param name="windowService">The window service.</param>
        /// <param name="objectProvider">The object provider.</param>
        /// <exception cref="System.ArgumentNullException">
        /// windowService
        /// or
        /// objectProvider
        /// </exception>
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
                throw new ArgumentNullException("windowService");
            if (objectProvider == null)
                throw new ArgumentNullException("objectProvider");

            var data = (Grids.BasicGrid)objectProvider.GetObject(); // error if it's not the correct object - fine

            using (var frm = new GridVisualizerForm(data))
            {
                windowService.ShowDialog(frm);
            }
        }

        /// <summary>
        /// Tests the visualizer by hosting it outside of the debugger.
        /// </summary>
        /// <param name="objectToVisualize">The object to display in the visualizer.</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(GridVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
