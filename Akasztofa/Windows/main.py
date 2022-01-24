import operator
import random
import time
from kivy import Config
from kivy.app import App
from kivy.lang import Builder
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.button import Button
from kivy.uix.image import Image
from kivy.uix.label import Label
from kivy.uix.screenmanager import Screen, ScreenManager
from kivy.uix.textinput import TextInput
from kivy.graphics import Color, Rectangle

Config.set("graphics", "width", "800")
Config.set("graphics", "height", "600")
Config.set("graphics", "resizable", "0")
Config.set("graphics", "borderless", "0")
Config.write()

kv = Builder.load_file("../KV-FILES/main.kv")


class MenuScreen(Screen):
    pass

class SummaryScreen(Screen):
    def back2game(self, button):
        self.parent.current = "jatek"
        self.parent.transition.direction = "right"

    def CreateWidgets(self):
        kep = Image(
            source='../Images/9.png',
            size_hint=(0.5, 0.5),
            pos_hint={"center_x": 0.5, "center_y": 0.5}
        )



        message = Label(
            text="[color=ff0000]Vesztettél!",
            pos_hint={"center_x": 0.5, "center_y": 0.95},
            size_hint=(0.05, 0.05),
            font_size='32sp',
            markup=True
        )

        global pontszamGlobal
        self.pontszamLbl = Label(
            text="[color=cc9c00]Pontszámod:" + pontszamGlobal,
            pos_hint={"center_x": 0.5, "center_y": 0.55},
            size_hint=(0.05, 0.05),
            font_size='32sp',
            markup=True
        )
        Back2Game = Button(
            text= "Új játék",
            size_hint= (0.4, 0.1),
            pos_hint= {"center_x": 0.5, "center_y": 0.205},
            background_color= (0, 1, 0),
            font_size='26sp'
        )
        Back2Game.bind(on_press=self.back2game)

        self.add_widget(kep)
        self.add_widget(message)
        self.add_widget(Back2Game)

class JatekWindow(Screen, BoxLayout):
    def ReadFromFile(self, path):
        f = open(path, "r")
        szavak = []
        for x in f:
            szavak.append(x)
        randomIndex = random.randint(0, len(szavak) - 1)
        randomSzo = szavak[randomIndex]
        self.hibaSzam = 0
        self.maradekHossz = len(randomSzo)-1
        return randomSzo

    def KeresBetu(self, betu):
        ujTomb = []
        voltBenne = False
        for c in self.keresettLbl.text:
            ujTomb.append(c)
        for c in range(0, len(self.randomSzo)-1):
            if self.randomSzo[c] == betu:
                voltBenne = True
                ujTomb[c*2] = betu
                self.maradekHossz -= 1
                if (self.maradekHossz == 0):
                    self.keresettLbl.text = "Nyertél! Keresett szó: "+self.randomSzo



        ujString = ""
        for c in ujTomb:
            ujString = ujString + c
        self.keresettLbl.text = ujString

        if self.hibaSzam <= 9 and not voltBenne:
            self.hibaSzam += 1
            self.kep.source = "../Images/"+str(self.hibaSzam)+".png"


        if (self.hibaSzam == 9):
            self.keresettLbl.text = "Vesztettél! Keresett szó: "+self.randomSzo

    def CreateWidgets(self):
        randSzo = self.ReadFromFile("../Data/szavak.txt")
        self.randomSzo = randSzo
        keresett = ""
        for i in range(0, len(self.randomSzo)-1):
            keresett = keresett + "_ "
        self.keresettLbl = Label(text="" ,
                    size=(200, 100),
                    font_size='24sp',
                    size_hint=(None, None),
                    pos_hint={"center_x": 0.5, "center_y": 0.4}
        )
        self.keresettLbl.text = keresett

        self.kep = Image(
            source='../Images/0.png',
            size_hint= (0.4, 0.4),
            pos_hint = {"center_x": 0.5, "center_y": 0.7}
        )
        self.add_widget(self.kep)

        self.add_widget(self.keresettLbl)
        abc = ["a", "á", "b", "c", "d", "e", "é", "f", "g", "h", "i", "í", "j", "k", "l", "m", "n", "o", "ó", "ö", "ő", "p", "r", "s", "t", "u", "ú", "ü", "ű", "v", "z"]
        for i in range(0, len(abc)): #31
            x = i / 11 + 0.04
            y = 0.25
            if i >= 11 and i < 21:
                y = 0.15
                x = (i - 10) / 11
            elif i >= 21:
                y = 0.05
                x = (i - 20) / 11

            button = Button(
                text=abc[i],
                size=(45, 45),
                pos_hint={"center_x": x, "center_y": y},
                size_hint=(None, None)
            )
            button.bind(on_press=self.keres)
            self.add_widget(button)

    def keres(self, button):
        betu = button.text
        print(button.text)
        button.disabled = True
        self.KeresBetu(betu)

class Manager(ScreenManager):
    pass

class Akasztofa(App):
    def build(self):
        return Manager()

Akasztofa().run()