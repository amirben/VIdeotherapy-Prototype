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
        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;

        private Gesture ContinuousGestureData = null;

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
                //this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }

            // load the gestures from the gesture database
            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(GestureResultView.CurrentExersice.DBPath))
            {
                this.vgbFrameSource.AddGestures(database.AvailableGestures);
                
                string continuousGestureName = this.GestureResultView.CurrentExersice.CurrentRound.ContinuousGesture.Name;
                foreach (var gesture in database.AvailableGestures)
                {
                    if (gesture.Name.Equals(continuousGestureName))
                    {
                        this.ContinuousGestureData = gesture;
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
        /// Gets or sets a value indicating whether or not the detector is currently paused
        /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                {
                    this.vgbFrameReader.IsPaused = value;
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
                    //this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
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
                            DiscreteGestureResult discreteResult = null;
                            ContinuousGestureResult continuousResult = null;

                            if (gesture.GestureType == GestureType.Discrete)
                            {
                                
                                discreteResults.TryGetValue(gesture, out discreteResult);

                                if (discreteResult != null)
                                {
                                    // check if start && and check and upate round
                                    //this.GestureResultView.UpdateDescreteGestureResult(true, gesture.Name, result);

                                    continuousResults.TryGetValue(this.ContinuousGestureData, out continuousResult);

                                    if (continuousResult != null)
                                    {
                                        // update progress bar
                                        progress = continuousResult.Progress;

                                        // clamp the progress value between 0 and 1
                                        if (progress < 0)
                                        {
                                            progress = 0;
                                        }
                                        else if (progress > 1)
                                        {
                                            progress = 1;
                                        }
                                    }
                                }
                            }

                            if (continuousResult != null && discreteResult != null)
                            {
                                this.GestureResultView.UpdateGestureResult(true, gesture.Name, discreteResult, progress);
                            }

                                                        
                        } // foreach
                    } // if
                } // if
            } //using

            //if exercise complete
            if (this.GestureResultView.CurrentExersice.IsPause)
            {
                IsPaused = true;
            }
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
