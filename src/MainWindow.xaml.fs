namespace Views

open FsXaml

type MainView = XAML<"MainWindow.xaml">

type CompositionRoot() =
    member __.ViewModel = ViewModel.MainViewModel(ScoreModel.Score.adjustScore)
