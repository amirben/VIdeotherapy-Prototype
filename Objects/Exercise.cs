using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VideoTherapyObjects
{
    public class Exercise
    {

        public String ExerciseName { get; set; }
        public int Repetitions { set; get; }
        public int ExerciseNumber { set; get; }
        public Gesture StartGesture { get; set; }
        public List<Gesture> GestureList { get; set; }
        public double ExerciseScore { set; get; }
        public String VideoPath { set; get; }
        public String ExerciseThumbs { set; get; }
    }
}
