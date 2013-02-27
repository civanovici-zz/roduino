'------------------------------------------------------------------------------'
'Program: miniaturowy miernik do zasilacza na attiny13 i LCD                   '
'Wersja z 64 próbkami i wyœwietlaniem œredniej                '
'kompilator: bascom 1.11.9.0     data: 23.12.2009    autor: Manekinen          '
'strona domowa projektu: http://diy.elektroda.eu/                              '
'wszelkie modyfikacje dozwolone, publikuj¹c nale¿y zachowaæ ten nag³ówek       '
'               WY£¥CZNIE DO U¯YTKU NIEKOMERCYJNEGO                            '
'------------------------------------------------------------------------------'

Const Pullup = 6                                            'ile mV b³êdu wprowadza pullup wyœwietlacza pod³¹czony do gnd poprzez 100ohm...  troche nieeleganckie ale proste i skuteczne :)

$regfile = "attiny13.dat"
'$regfile = "m8def.dat"
$crystal = 1200000
'$crystal = 2000000
$hwstack = 8
$swstack = 16
$framesize = 16
'$noramclear
'$sim

Dim Pomiar As Word
Dim Pokaz As String * 4
Dim Pokaz2 As String * 4
Dim Znak As Byte
Dim Licz As Byte
Dim Petla As Byte

Declare Sub Mierz
Declare Sub Formuj
Declare Sub Wyswietl

Config Adc = Single , Prescaler = Auto , Reference = Internal       'off
Start Adc

Config Lcd = 16 * 2
Config Lcdpin = Pin , Db4 = Portb.2 , Db5 = Portb.4 , Db6 = Portb.3 , Db7 = Portb.5 , E = Portb.1 , Rs = Portb.0
Cursor Off
Cls


Do

   Upperline                                                'od nowa dla napiêcia i pr¹du
   Licz = 1                                                 'adc dla napiecia
   Mierz                                                    'mierzy i formuje napiêcie
   'Moc = Pomiar                                             'kopia dla obliczeñ mocy
   Znak = 86                                                'znak dla napiêcia
   Wyswietl                                                 'wyœwietla napiêcie

   Licz = 3                                                 'adc dla pr¹du ------- ??????
   Mierz                                                    'mierzy i formuje pr¹d
   'w pomiar zostaje pr¹d
   Pokaz2 = Format(pokaz , "0.00")                          'forumje pr¹d drugi raz
   Znak = 65                                                'znak dla pr¹du
   Wyswietl                                                 'wyœwietla pr¹d

   'Lowerline                                                'od nowa dla temperatury i mocy
   'Mnozenie
   'Pomiar = Moc * Pomiar                                    'liczy moc
   'Pomiar = Pomiar / 100
   'Formuj
   'Znak = 87                                                'znak dla mocy
   'Wyswietl                                                 'wyœwietla moc

   Licz = 0                                                 'adc dla temperatury
   Mierz                                                    'mierzy i formuje temperaturê
   Znak = 42                                                'znak dla temperatury
   Wyswietl                                                 'wyœwietla temperaturê


Loop
'_______________________
Mierz:
Pomiar = 0
Petla = 0

Do
Incr Petla
Pomiar = Pomiar + Getadc(licz)                              'sumuje pomiary
Pomiar = Pomiar - Pullup                                    'za ka¿dym razem odejmuje napiêcie pullupa
Loop Until Petla = 64                                       '64 próbki

Shift Pomiar , Right , 6                                    '6/64
If Pomiar > 999 Then Pomiar = 0                             'granica do 99.9


Formuj:

Pokaz = Str(pomiar)
Pokaz2 = Format(pokaz , "00.0")
Return


Wyswietl:
Ddrb = &B111111
Lcd Pokaz2 ; Chr(znak) ; Chr(32)
Ddrb = &B010011
Return
