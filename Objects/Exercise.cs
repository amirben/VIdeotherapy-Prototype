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
        public String DBPath { get; set; }
        public String VideoPath { set; get; }
        public String ExerciseThumbs { set; get; }
        
        public int Repetitions { set; get; }
        public int ExerciseNumber { set; get; }
        public ExerciseGesture StartGesture { set; get; }
        public List<Round> Rounds { get; set; }
        public double ExerciseScore { set; get; }
        
        private bool isPause = false;
        private bool isStarted = false;

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
                // debug:
                printAllRoundStatus();

                CurrentRound = Rounds[++currentRound];
            }
        }

        public bool IsPause
        {
            set
            {
                this.isPause = value;
            }
            get
            {
                return this.isPause;
            }
        }

        public bool IsStarted
        {
            set
            {
                this.isStarted = value;
            }
            get
            {
                return this.isStarted;
            }
        }

        private void printAllRoundStatus()
        {
            Console.WriteLine("Round #{0}", currentRound);

            foreach (ExerciseGesture gesture in CurrentRound.GestureList.Values)
            {
                Console.WriteLine(gesture.Name + " " + gesture.SuccesStatus + " \n");
            }
        }

        //Only for read XML:
        public List<ExerciseGesture> ListG { set; get; }
        public ExerciseGesture ContinuousGestureTemp { set; get; }

        public void CreateRounds()
        {
            Rounds = new List<Round>();
            for (int i = 0; i < Repetitions; i++)
            {
                Rounds.Add(new Round());

                foreach (ExerciseGesture gesture in ListG)
                {
                    Rounds[i].GestureList.Add(gesture.Name, gesture);
                }

                Rounds[i].ContinuousGesture = new ExerciseGesture(ContinuousGestureTemp);
                
            }
        }
    }
}
