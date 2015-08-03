using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
namespace VideoTherapyObjects
{
    public class Round
    {
        
        public Dictionary<String, ExerciseGesture> GestureList { set; get; }
        
        public ExerciseGesture ContinuousGesture { set; get; }
        public double RoundProgress { set; get; }

        public Round()
        {
            GestureList = new Dictionary<string,ExerciseGesture>();
        }

        private bool _roundSuccess = false;
        public bool RoundSuccess
        {

            get
            {
                //// check if round finshed
                //foreach (ExerciseGesture gesture in GestureList.Values)
                //{
                //    if (!gesture.SuccesStatus)
                //    {
                //        _roundSuccess = false;
                //        return _roundSuccess;
                //    }

                //}

                //_roundSuccess = true;
                return _roundSuccess;
            }

            set
            {
                _roundSuccess = value;
            }

        }

        public bool CheckRoundSuccess()
        {
            // check if round finshed
            foreach (ExerciseGesture gesture in GestureList.Values)
            {
                if (!gesture.SuccesStatus)
                {
                    return false;
                }

            }

            return true;
        }
        public void UpdateCompeleteGesture(string gestureName, DiscreteGestureResult result, float progress)
        {
            
            if (GestureList.ContainsKey(gestureName))
            {
                ExerciseGesture gesture = null;
                GestureList.TryGetValue(gestureName, out gesture);
       
                gesture.ConfidenceValue = result.Confidence;

                if (result.Detected)
                {
                    gesture.ProgressValue = progress;
                }

                if (result.Detected && gesture.ConfidenceValue > 0.3f)
                {
                    //Console.WriteLine("{0}: Conf: {1}, Detected: {2}, Progress: {3}", gestureName, gesture.ConfidenceValue.ToString(), result.Detected.ToString(), gesture.ProgressValue.ToString());
                    gesture.SuccesStatus = true;
                }
            }
        }
    }
}
