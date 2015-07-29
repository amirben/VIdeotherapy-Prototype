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


        public bool RoundSuccess
        {

            get
            {
                foreach (ExerciseGesture gesture in GestureList.Values)
                {
                    if (!gesture.SuccesStatus)
                    {
                        return false;
                    }

                }

                return true;
            }

        }

        public void UpdateCompeleteGesture(string gestureName, DiscreteGestureResult result)
        {
            if (GestureList.ContainsKey(gestureName))
            {
                GestureList[gestureName].SuccesStatus = result.Detected;
                GestureList[gestureName].ScoreValue = result.Confidence;
            }
        }

       
        
    }
}
