namespace ViewModels

open System
open System.Windows
open FSharp.ViewModule
open FsXaml

type MainView = XAML<"MainWindow.xaml", true>

type Score = {
    ScoreA: int
    ScoreB: int
}

type ScoringEvent = IncA | DecA | IncB | DecB | New

type MainViewModel() as self = 
    inherit EventViewModelBase<ScoringEvent>()

    let defaultScore = { ScoreA = 0; ScoreB = 0}
    let mutable score = defaultScore

    let eventHandler ev =
        match ev with
        | IncA -> score <- {score with ScoreA = score.ScoreA + 1}
        | DecA -> score <- {score with ScoreA = score.ScoreA - 1}
        | IncB -> score <- {score with ScoreB = score.ScoreB + 1}
        | DecB -> score <- {score with ScoreB = score.ScoreB + 1}
        | New -> score <- defaultScore

    do
        self.EventStream
        |> Observable.subscribe eventHandler
        |> ignore

    member self.IncA = self.Factory.EventValueCommand(IncA)
    member self.DecA = self.Factory.EventValueCommand(DecA)
    member self.IncB = self.Factory.EventValueCommand(IncB)
    member self.DecB = self.Factory.EventValueCommand(DecB)
    member self.NewGame = self.Factory.EventValueCommand(New)

    member self.ScoreA = score.ScoreA.ToString("D2")
    member self.ScoreB = score.ScoreB.ToString("D2")
