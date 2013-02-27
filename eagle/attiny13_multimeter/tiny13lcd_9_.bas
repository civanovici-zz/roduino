'------------------------------------------------------------------------------'
'Program: miniaturowy miernik do zasilacza na procesorze attiny13              '
'kompilator: bascom 1.11.9.0     data: 21.12.2009    autor: Manekinen          '
'strona domowa projektu: http://diy.elektroda.eu/                              '
'wszelkie modyfikacje dozwolone, publikuj¹c nale¿y zachowaæ ten nag³ówek       '
'               WY£¥CZNIE DO U¯YTKU NIEKOMERCYJNEGO                            '
'------------------------------------------------------------------------------'

Const Napiecie = 62                                         'mno¿nik napiêcia (62 dla 100V... 19 dla 30V... itp)
Const Prad = 62                                             'mno¿nik pr¹du (62 dla 10A... j.w.)
Const Temp = 62                                             'mno¿nik temperatury, dobrany eksperymentalnie dla vref 1.1V i LM35 (wliczaj¹c mV poprawkê poni¿ej)
Const Pullup = 6                                            'ile mV b³êdu wprowadza pullup wyœwietlacza pod³¹czony do gnd poprzez 100ohm...  troche nieeleganckie ale proste i skuteczne :)

$regfile = "attiny13.dat"
'$regfile = "m8def.dat"
$crystal = 1200000
'$crystal = 2000000
$hwstack = 16
$swstack = 16
$framesize = 16
$noramclear

Dim Pomiar As Word
Dim Pokaz As String * 4
Dim Pokaz2 As String * 4
Dim Znak As Byte
Dim Licz As Byte

Declare Sub Formuj_i_mierz
Declare Sub Wyswietl

Config Adc = Single , Prescaler = Auto , Reference = Internal       'off
Start Adc

Config Lcd = 16 * 1
Config Lcdpin = Pin , Db4 = Portb.2 , Db5 = Portb.4 , Db6 = Portb.3 , Db7 = Portb.5 , E = Portb.1 , Rs = Portb.0
'Config Lcdpin = Pin , Db4 = Portc.0 , Db5 = Portc.1 , Db6 = Portc.2 , Db7 = Portc.3 , E = Portb.1 , Rs = Portb.0
Cursor Off

Do
   Upperline

   Znak = 86
   Pomiar = Pomiar * Napiecie
   'pr¹d...
   Licz = 3
   Formuj_i_mierz
   Wyswietl
   Znak = 65
   Pomiar = Pomiar * Prad
   'temperatura...
   Licz = 0
   Formuj_i_mierz
   Pokaz2 = Format(pokaz , "0.00")
   Wyswietl
   Pomiar = Pomiar * Temp
   'napiêcie...
   Licz = 1
   Formuj_i_mierz
   Wyswietl
Loop

Formuj_i_mierz:

'Ddrc = &B110001
'        543210
Ddrb = &B010011

Shift Pomiar , Right , 6
Pokaz = Str(pomiar)
Pokaz2 = Format(pokaz , "00.0")

Waitms 50
Pomiar = Getadc(licz) - Pullup
Return

Wyswietl:
'Ddrc = &B111111
'        543210
Ddrb = &B111111
Lcd Pokaz2 ; Chr(znak) ; Chr(32)
Return