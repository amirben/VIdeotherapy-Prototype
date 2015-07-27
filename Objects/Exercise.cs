using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VideoTherapyObjects
{
    public class Exercise
    {
        private int currentRound = 0;

        public String ExerciseName { get; set; }
        public int Repetitions { set; get; }
        public int ExerciseNumber { set; get; }
        public ExerciseGesture StartGesture { set; get; }
        public List<Round> Rounds { get; set; }
        public double ExerciseScore { set; get; }
        public String VideoPath { set; get; }
        public String ExerciseThumbs { set; get; }
        
        public Round CurrentRound { set; get; }

        public String RoundsLeft
        {
            get
            {
                return String.Format("{0} / {1} Rounds", currentRound, Repetitions);
            }
        }

        public void NextRound()
        {
            if (currentRound < Repetitions)
            {
                CurrentRound = Rounds[currentRound];
            }
        }

       
    }
}
