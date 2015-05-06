namespace ViewModels

open System
open System.Windows
open FSharp.ViewModule
open FsXaml

type MainView = XAML<"MainWindow.xaml", true>

type Score = {
    ScoreA: int
    ScoreB: int
} with 
    static member zero = {ScoreA = 0; ScoreB = 0}


type ScoringEvent = IncA | DecA | IncB | DecB | New

type Controller = Score -> ScoringEvent -> Score


type MainViewModel(controller : Controller) as self = 
    inherit EventViewModelBase<ScoringEvent>()

    let mutable score = Score.zero

    let scoreA = self.Factory.Backing(<@ self.ScoreA @>, "00")
    let scoreB = self.Factory.Backing(<@ self.ScoreB @>, "00")

    let updateView () =
        self.ScoreA <- score.ScoreA.ToString("D2")
        self.ScoreB <- score.ScoreB.ToString("D2")

    let eventHandler ev =
        score <- controller score ev

    do
        self.EventStream
        |> Observable.subscribe (eventHandler >> updateView)
        |> ignore

    member self.IncA = self.Factory.EventValueCommand(IncA)
    member self.DecA = self.Factory.EventValueCommand(DecA)
    member self.IncB = self.Factory.EventValueCommand(IncB)
    member self.DecB = self.Factory.EventValueCommand(DecB)
    member self.NewGame = self.Factory.EventValueCommand(New)

    member self.ScoreA with get() = scoreA.Value 
                        and set value = scoreA.Value <- value
    member self.ScoreB with get() = scoreB.Value 
                        and set value = scoreB.Value <- value


module Handling = 

    let controller score ev =
       match ev with
        | IncA -> {score with ScoreA = score.ScoreA + 1}
        | DecA -> {score with ScoreA = score.ScoreA - 1}
        | IncB -> {score with ScoreB = score.ScoreB + 1}
        | DecB -> {score with ScoreB = score.ScoreB - 1}
        | New -> Score.zero 


type CompositionRoot() =
    member x.ViewModel with get() = MainViewModel(Handling.controller)
