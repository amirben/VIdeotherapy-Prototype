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
        public Boolean SuccesStatus { set; get; }
        public GestureType Type { set; get; }
        public float minValue { set; get; }
        public float maxValue { set; get; }
        

        private float _progressValue = 0;
        public float ProgressValue
        {
            get
            {
                return _progressValue;
            }

            set
            {
                if (maxValue > minValue) // from 0 to 1
                {
                    if (_progressValue < value)
                    {
                        _progressValue = value;
                    }
                }
                else // from 1 to 0
                {
                    if (_progressValue > value)
                    {
                        _progressValue = value;
                    }
                }
                // if (Math.Abs(maxValue - value) < 0.1f)
                //{
                //    _progressValue = value;
                //}
            }
        }

        private float _confidence = 0.0f;
        public float ConfidenceValue
        {
            get
            {
                return _confidence;
            }

            set
            {
                if (value > _confidence)
                {
                    _confidence = value;
                }
            }
        }

        public ExerciseGesture() 
        {
           
        }

        public ExerciseGesture(ExerciseGesture oldGesture)
        {
            Name = oldGesture.Name;
            SuccesStatus = oldGesture.SuccesStatus;
            ConfidenceValue = oldGesture.ConfidenceValue;
            minValue = oldGesture.minValue;
            maxValue = oldGesture.maxValue;
            Type = oldGesture.Type;

            initProgress();
        }

        public void initProgress()
        {
            _progressValue = (maxValue < minValue) ? Math.Max(minValue, maxValue) : Math.Min(minValue, maxValue);
        }
    }
}
