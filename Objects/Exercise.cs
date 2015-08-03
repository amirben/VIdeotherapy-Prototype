using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace VideoTherapyObjects
{
    public class Exercise : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; 

        public int currentRound = 0;

        public String ExerciseName { get; set; }
        public String DBPath { get; set; }
        public String VideoPath { set; get; }
        public String ExerciseThumbs { set; get; }
        
        public int Repetitions { set; get; }
        public int ExerciseNumber { set; get; }
        public ExerciseGesture StartGesture { set; get; }
        public ObservableCollection<Round> Rounds { get; set; }
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
            
            if (!isPause)
            {
                // debug:
                printAllRoundStatus();

                currentRound++;

                if (currentRound < Repetitions)
                {
                    CurrentRound.RoundSuccess = true;
                    CurrentRound = Rounds[currentRound];
                }
                else
                {
                    // todo finish exercise
                    Console.WriteLine("Exercise complete");
                    isPause = true;
                }

                this.NotifyPropertyChanged("RoundsLeft");
                this.NotifyPropertyChanged("RoundSuccess");
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
                Console.WriteLine(gesture.Name + " " + gesture.SuccesStatus + " Confidence: " + gesture.ConfidenceValue + " Progress: " + gesture.ProgressValue);
            }
        }

        //Only for read XML:
        public List<ExerciseGesture> ListG { set; get; }
        public ExerciseGesture ContinuousGestureTemp { set; get; }

        public void CreateRounds()
        {
            Rounds = new ObservableCollection<Round>();
            for (int i = 0; i < Repetitions; i++)
            {
                Rounds.Add(new Round());

                foreach (ExerciseGesture gesture in ListG)
                {
                    Rounds[i].GestureList.Add(gesture.Name, new ExerciseGesture(gesture));
                }

                Rounds[i].ContinuousGesture = new ExerciseGesture(ContinuousGestureTemp);
                
            }

            CurrentRound = Rounds[0];
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
