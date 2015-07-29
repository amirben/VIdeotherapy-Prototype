using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect.VisualGestureBuilder;

namespace VideoTherapyObjects
{
    
    
    public class ExerciseGesture
    {
        public enum ExerciseGestureType { Discrete, Progress };

        public String Name { set; get; }
        public double ScoreValue { set; get; }
        public Boolean SuccesStatus { set; get; }
        public double minValue { set; get; }
        public double maxValue { set; get; }
        public GestureType Type { set; get; }

        public ExerciseGesture() {}

        public ExerciseGesture(ExerciseGesture oldGesture)
        {
            Name = oldGesture.Name;
            SuccesStatus = oldGesture.SuccesStatus;
            ScoreValue = oldGesture.ScoreValue;
            minValue = oldGesture.minValue;
            maxValue = oldGesture.maxValue;
            Type = oldGesture.Type;
        }
    }
}
