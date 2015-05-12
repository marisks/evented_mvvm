namespace ViewModel

open ScoreModel
open FSharp.ViewModule

type MainViewModel(controller : Score -> ScoringEvent -> Score) as self = 
    inherit EventViewModelBase<ScoringEvent>()

    let score = self.Factory.Backing(<@ self.Score @>, Score.zero)

    let eventHandler ev = score.Value <- controller score.Value ev

    do
        self.EventStream
        |> Observable.add eventHandler

    member this.IncA = this.Factory.EventValueCommand(IncA)
    member this.DecA = this.Factory.EventValueCommandChecked(DecA, (fun _ -> this.Score.ScoreA > 0), [ <@@ this.Score @@> ])
    member this.IncB = this.Factory.EventValueCommand(IncB)
    member this.DecB = this.Factory.EventValueCommandChecked(DecB, (fun _ -> this.Score.ScoreB > 0), [ <@@ this.Score @@> ])
    member this.NewGame = this.Factory.EventValueCommand(New)

    member __.Score = score.Value 
