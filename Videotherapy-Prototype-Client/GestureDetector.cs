using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;


namespace VideotherapyPrototype
{
    

    /// <summary>
    /// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
    /// and updates the associated GestureResultView object with the latest results for the 'Seated' gesture
    /// </summary>
    public class GestureDetector : IDisposable
    {
        /// <summary> Path to the gesture database that was trained with VGB </summary>
        private readonly string gestureDatabase = @"C:\Users\Ben\Desktop\Afeka\VideoTherapy\Kinect Tests\ContinousGestureBasics-WPF\Database\ClappingHands.gbd";

        private const string VGB_DATABASE_FILE = @"Database\ClappingHands.gbd";

        /// <summary> Name of the discrete gesture in the database that we want to track </summary>
        private readonly string seatedGestureName = "ClappingHands";

        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;

        /// <summary>
        /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
        /// </summary>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
        /// <param name="gestureResultView">GestureResultView object to store gesture results of a single body to</param>
        public GestureDetector(KinectSensor kinectSensor, GestureResultView gestureResultView)
        {
            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");
            }

            if (gestureResultView == null)
            {
                throw new ArgumentNullException("gestureResultView");
            }
            
            this.GestureResultView = gestureResultView;
            
            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

            // open the reader for the vgb frames
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
                this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }

            // load the 'Seated' gesture from the gesture database
            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(gestureDatabase))
            {
                // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
                // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
                foreach (Gesture gesture in database.AvailableGestures)
                {
                    if (gesture.Name.Equals(this.seatedGestureName))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }
                }
            }
        }

        /// <summary> Gets the GestureResultView object which stores the detector results for display in the UI </summary>
        public GestureResultView GestureResultView { get; private set; }

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector
        /// The tracking ID can change whenever a body comes in/out of scope
        /// </summary>
        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                {
                    this.vgbFrameSource.TrackingId = value;
                }
            }
        }


        /// <summary>
        /// Disposes all unmanaged resources for the class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        /// </summary>
        /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.vgbFrameReader != null)
                {
                    this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                    this.vgbFrameReader.Dispose();
                    this.vgbFrameReader = null;
                }

                if (this.vgbFrameSource != null)
                {
                    this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                    this.vgbFrameSource.Dispose();
                    this.vgbFrameSource = null;
                }
            }
        }

        /// <summary>
        /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            //VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            //using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            //{
            //    if (frame != null)
            //    {
            //        // get the discrete gesture results which arrived with the latest frame
            //        IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

            //        // get the progress gesture results which arrived with the latest frame
            //        IReadOnlyDictionary<Gesture, ContinuousGestureResult> continuousResults = frame.ContinuousGestureResults;

            //        if (continuousResults != null && discreteResults != null)
            //        {
                        


            //            // we only have one gesture in this source object, but you can get multiple gestures
            //            foreach (Gesture gesture in this.vgbFrameSource.Gestures)
            //            {
            //                if (gesture.Name.Equals(this.seatedGestureName) && gesture.GestureType == GestureType.Continuous)
            //                {
                                
                               
            //                    ContinuousGestureResult result = null;
            //                    continuousResults.TryGetValue(gesture, out result);
                                
            //                    if (result != null)
            //                    {
            //                        // update the GestureResultView object with new gesture result values
            //                      //  this.GestureResultView.UpdateGestureResult(true, result.Detected, result.Confidence);
            //                        var progress = result.Progress;
                                    
            //                        if (progress > 1.48 && progress < 3 )
            //                        {
            //                            //we're clapping but not finished
            //                            this.GestureResultView.UpdateGestureResult(true, true, 1.0f);
            //                        }
            //                        else
            //                        {
            //                            this.GestureResultView.UpdateGestureResult(true, false, 1.0f);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }


        public void GestureDataAnalyze()
        {
            float progress = 0;

            using (var frame = this.vgbFrameReader.CalculateAndAcquireLatestFrame())
            {
                if (frame != null)
                {
                    // get all discrete and continuous gesture results that arrived with the latest frame
                    var discreteResults = frame.DiscreteGestureResults;
                    var continuousResults = frame.ContinuousGestureResults;

                    if (discreteResults != null)
                    {
                        foreach (var gesture in this.vgbFrameSource.Gestures)
                        {
                            if (gesture.GestureType == GestureType.Discrete)
                            {
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                
                                if (result != null)
                                {
                                    // check if start && and check and upate round
                                    UpdateRoundData(result, gesture);

                                }
                            }

                            if (continuousResults != null)
                            {
                                if (gesture.GestureType == GestureType.Continuous /* && check the name of the gesture */)
                                {
                                    ContinuousGestureResult result = null;
                                    continuousResults.TryGetValue(gesture, out result);

                                    if (result != null)
                                    {
                                        // update progress bar
                                        progress = result.Progress;
                                    }

                                } //if
                            } // if
                        } // foreach
                    } // if
                } // if
            } //using


            // clamp the progress value between 0 and 1
            if (progress < 0)
            {
                progress = 0;
            }
            else if (progress > 1)
            {
                progress = 1;
            }

            // update the progress result
            this.GestureResultView.UpdateContinuousGestureResult(true, progress);
        }

        public void UpdateRoundData(DiscreteGestureResult result, Gesture gesture)
        {
            // check if it start gesture
            // if start gesture - update UI countdown
            // if (CurrentExercise.StartGesture.Name.Equals(gesture.name)) {}

            // else  - if start gesture already been set
            // CurrentExercise.Round.CompeleteGesture(gesture.Name, result);

            // check if round complete
            // if so, update UI

            // if (CurrentExercise.Round.RoundSuccess) { CurrentExercise.NextRound()}
        }


        /// <summary>
        /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
        {
            // update the GestureResultView object to show the 'Not Tracked' image in the UI
            this.GestureResultView.UpdateContinuousGestureResult(false, 0.0f);
        }
    }
}
