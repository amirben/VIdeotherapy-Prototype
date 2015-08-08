using VideoTherapyObjects;
using Microsoft.Kinect.VisualGestureBuilder;
using VideotherapyPrototype;
using VideotherapyPrototype.Switcher;

namespace VideotherapyPrototype
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    
    /// <summary>
    /// Stores discrete gesture results for the GestureDetector.
    /// Properties are stored/updated for display in the UI.
    /// </summary>
    public sealed class GestureResultView : INotifyPropertyChanged
    {
        /// <summary> Image to show when the 'detected' property is true for a tracked body </summary>
        private readonly ImageSource seatedImage = new BitmapImage(new Uri(@"Images\Clapping.png", UriKind.Relative));

        /// <summary> Image to show when the 'detected' property is false for a tracked body </summary>
        private readonly ImageSource notSeatedImage = new BitmapImage(new Uri(@"Images\NotClapping.png", UriKind.Relative));

        /// <summary> Image to show when the body associated with the GestureResultView object is not being tracked </summary>
        private readonly ImageSource notTrackedImage = new BitmapImage(new Uri(@"Images\NotTracked.png", UriKind.Relative));

        /// <summary> The value of the progress of current round </summary>
        private float progress = 0.0f;

        /// <summary> Image to display in UI which corresponds to tracking/detection state </summary>
        private ImageSource imageSource = null;
        
        /// <summary> True, if the body is currently being tracked </summary>
        private bool isTracked = false;

        /// <summary> Current play exercise </summary>
        private Exercise currentExercise = null;

        //DELEGATE:
        public event VideotherapyPrototype.UC_Exercise.RoundSuccessDelegate roundSuccessEvent;
        public event VideotherapyPrototype.UC_Exercise.StartDelegate StartEvent;

        /// <summary>
        /// Initializes a new instance of the GestureResultView class and sets initial property values
        /// </summary>
        /// <param name="bodyIndex">Body Index associated with the current gesture detector</param>
        /// <param name="isTracked">True, if the body is currently tracked</param>
        /// <param name="detected">True, if the gesture is currently detected for the associated body</param>
        /// <param name="confidence">Confidence value for detection of the 'Seated' gesture</param>
        public GestureResultView(bool isTracked, float progress, Exercise currentExercise)
        {
            if (currentExercise == null)
            {
                throw new ArgumentNullException("No exercise");
            }

            this.IsTracked = isTracked;
            this.progress = progress;
            this.currentExercise = currentExercise;

            this.ImageSource = this.notTrackedImage;
        }

        public void UpdateContinuousGestureResult(bool isBodyTrackingIdValid, float progress)
        {
            this.isTracked = isBodyTrackingIdValid;

            if (!this.isTracked)
            {
                // throw error the UC-Exercise

            }
            else
            {
                this.currentExercise.CurrentRound.RoundProgress = progress;
                
                if (this.currentExercise.CurrentRound.RoundSuccess)
                {
                    this.currentExercise.NextRound();
                }
            }
        }

        public void UpdateStartGestureResult(bool isBodyTrackIdValid, string gestureName, DiscreteGestureResult result)
        {

        }

        public void UpdateGestureResult(bool isBodyTrackingIdValid, string gestureName, DiscreteGestureResult result, float progress)
        {
            this.isTracked = isBodyTrackingIdValid;

            if (!this.isTracked)
            {
                // throw error the UC-Exercise

            }
            else
            {
                //if it start condition
                if (!this.currentExercise.IsStarted)
                {
                    if (gestureName.Equals(this.currentExercise.StartGesture.Name))
                    {
                        if (result.Detected /*&& result.Confidence > 0.3f*/)
                        {
                            this.currentExercise.StartGesture.SuccesStatus = result.Detected;
                            this.currentExercise.StartGesture.ConfidenceValue = result.Confidence;
                            this.currentExercise.IsStarted = true;

                            Console.WriteLine("Test: > Start gesture recognized");
                            //todo Start UI - countdown
                            StartEvent();
                        }

                        else
                        {
                            //Console.WriteLine("Please be in start position");
                        }
                    }
                }

                else
                {
                    // check other gesture
                    this.currentExercise.CurrentRound.UpdateCompeleteGesture(gestureName, result, progress);


                    // check if round finshed
                    if (progress <= 0.01f && this.currentExercise.CurrentRound.CheckRoundSuccess() && !this.currentExercise.ExerciseComplete)
                    {

                        
                        //Console.WriteLine("Round #{0} Finished! Status: {1}", this.currentExercise.currentRound, this.currentExercise.CurrentRound.RoundSuccess);
                        this.currentExercise.NextRound();
                        roundSuccessEvent();

                    }

                    if (this.currentExercise.ExerciseComplete)
                    {
                        Switcher.Switcher.Switch(new UC_ExerciseResult(this.currentExercise));
                    }
                }
            }
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> 
        /// Gets a value indicating whether or not the body associated with the gesture detector is currently being tracked 
        /// </summary>
        public bool IsTracked 
        {
            get
            {
                return this.isTracked;
            }

            private set
            {
                if (this.IsTracked != value)
                {
                    this.isTracked = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        /// <summary> 
        /// Gets an image for display in the UI which represents the current gesture result for the associated body 
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }

            private set
            {
                if (this.ImageSource != value)
                {
                    this.imageSource = value;
                    this.NotifyPropertyChanged();
                }
            }
        }


        /// <summary> 
        /// Gets a value indicating the progress that the user had 
        /// </summary>
        public float Progress
        {
            get
            {
                return this.progress;
            }

            private set
            {
                if (this.progress != value)
                {
                    this.progress = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public Exercise CurrentExersice
        {
            get
            {
                return currentExercise;
            }
        }
        /// <summary>
        /// Notifies UI that a property has changed
        /// </summary>
        /// <param name="propertyName">Name of property that has changed</param> 
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
