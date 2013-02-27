<?xml version="1.0" encoding="utf-8"?>
<!DOCTYPE eagle SYSTEM "eagle.dtd">
<eagle version="6.0">
<drawing>
<settings>
<setting alwaysvectorfont="no"/>
<setting verticaltext="up"/>
</settings>
<grid distance="0.1" unitdist="inch" unit="inch" style="lines" multiple="1" display="no" altdistance="0.01" altunitdist="inch" altunit="inch"/>
<layers>
<layer number="1" name="Top" color="4" fill="1" visible="no" active="no"/>
<layer number="16" name="Bottom" color="1" fill="1" visible="no" active="no"/>
<layer number="17" name="Pads" color="2" fill="1" visible="no" active="no"/>
<layer number="18" name="Vias" color="2" fill="1" visible="no" active="no"/>
<layer number="19" name="Unrouted" color="6" fill="1" visible="no" active="no"/>
<layer number="20" name="Dimension" color="15" fill="1" visible="no" active="no"/>
<layer number="21" name="tPlace" color="7" fill="1" visible="no" active="no"/>
<layer number="22" name="bPlace" color="7" fill="1" visible="no" active="no"/>
<layer number="23" name="tOrigins" color="15" fill="1" visible="no" active="no"/>
<layer number="24" name="bOrigins" color="15" fill="1" visible="no" active="no"/>
<layer number="25" name="tNames" color="7" fill="1" visible="no" active="no"/>
<layer number="26" name="bNames" color="7" fill="1" visible="no" active="no"/>
<layer number="27" name="tValues" color="7" fill="1" visible="no" active="no"/>
<layer number="28" name="bValues" color="7" fill="1" visible="no" active="no"/>
<layer number="29" name="tStop" color="7" fill="3" visible="no" active="no"/>
<layer number="30" name="bStop" color="7" fill="6" visible="no" active="no"/>
<layer number="31" name="tCream" color="7" fill="4" visible="no" active="no"/>
<layer number="32" name="bCream" color="7" fill="5" visible="no" active="no"/>
<layer number="33" name="tFinish" color="6" fill="3" visible="no" active="no"/>
<layer number="34" name="bFinish" color="6" fill="6" visible="no" active="no"/>
<layer number="35" name="tGlue" color="7" fill="4" visible="no" active="no"/>
<layer number="36" name="bGlue" color="7" fill="5" visible="no" active="no"/>
<layer number="37" name="tTest" color="7" fill="1" visible="no" active="no"/>
<layer number="38" name="bTest" color="7" fill="1" visible="no" active="no"/>
<layer number="39" name="tKeepout" color="4" fill="11" visible="no" active="no"/>
<layer number="40" name="bKeepout" color="1" fill="11" visible="no" active="no"/>
<layer number="41" name="tRestrict" color="4" fill="10" visible="no" active="no"/>
<layer number="42" name="bRestrict" color="1" fill="10" visible="no" active="no"/>
<layer number="43" name="vRestrict" color="2" fill="10" visible="no" active="no"/>
<layer number="44" name="Drills" color="7" fill="1" visible="no" active="no"/>
<layer number="45" name="Holes" color="7" fill="1" visible="no" active="no"/>
<layer number="46" name="Milling" color="3" fill="1" visible="no" active="no"/>
<layer number="47" name="Measures" color="7" fill="1" visible="no" active="no"/>
<layer number="48" name="Document" color="7" fill="1" visible="no" active="no"/>
<layer number="49" name="Reference" color="7" fill="1" visible="no" active="no"/>
<layer number="51" name="tDocu" color="7" fill="1" visible="no" active="no"/>
<layer number="52" name="bDocu" color="7" fill="1" visible="no" active="no"/>
<layer number="91" name="Nets" color="2" fill="1" visible="yes" active="yes"/>
<layer number="92" name="Busses" color="1" fill="1" visible="yes" active="yes"/>
<layer number="93" name="Pins" color="2" fill="1" visible="no" active="yes"/>
<layer number="94" name="Symbols" color="4" fill="1" visible="yes" active="yes"/>
<layer number="95" name="Names" color="7" fill="1" visible="yes" active="yes"/>
<layer number="96" name="Values" color="7" fill="1" visible="yes" active="yes"/>
<layer number="97" name="Info" color="7" fill="1" visible="yes" active="yes"/>
<layer number="98" name="Guide" color="6" fill="1" visible="yes" active="yes"/>
</layers>
<schematic xreflabel="%F%N/%S.%C%R" xrefpart="/%S.%C%R">
<libraries>
<library name="Microchip-MCP73871-2AAI_ML">
<packages>
<package name="QFN50P400X400X100-21N">
<smd name="1" x="-1.8796" y="0.9906" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="2" x="-1.8796" y="0.508" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="3" x="-1.8796" y="0" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="4" x="-1.8796" y="-0.508" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="5" x="-1.8796" y="-0.9906" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="6" x="-0.9906" y="-1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="7" x="-0.508" y="-1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="8" x="0" y="-1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="9" x="0.508" y="-1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="10" x="0.9906" y="-1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="11" x="1.8796" y="-0.9906" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="12" x="1.8796" y="-0.508" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="13" x="1.8796" y="0" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="14" x="1.8796" y="0.508" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="15" x="1.8796" y="0.9906" dx="0.3048" dy="0.8128" layer="1" rot="R270"/>
<smd name="16" x="0.9906" y="1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="17" x="0.508" y="1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="18" x="0" y="1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="19" x="-0.508" y="1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="20" x="-0.9906" y="1.8796" dx="0.3048" dy="0.8128" layer="1" rot="R180"/>
<smd name="21" x="0" y="0" dx="2.7432" dy="2.7432" layer="1"/>
<wire x1="1.9812" y1="1.4732" x2="1.9812" y2="1.9812" width="0.1524" layer="21"/>
<wire x1="1.4732" y1="-1.9812" x2="1.9812" y2="-1.9812" width="0.1524" layer="21"/>
<wire x1="-1.4732" y1="1.9812" x2="-1.9812" y2="1.9812" width="0.1524" layer="21"/>
<wire x1="-1.9812" y1="-1.9812" x2="-1.4732" y2="-1.9812" width="0.1524" layer="21"/>
<wire x1="1.9812" y1="-1.9812" x2="1.9812" y2="-1.4732" width="0.1524" layer="21"/>
<wire x1="1.9812" y1="1.9812" x2="1.4732" y2="1.9812" width="0.1524" layer="21"/>
<wire x1="-1.9812" y1="1.9812" x2="-1.9812" y2="1.4732" width="0.1524" layer="21"/>
<wire x1="-1.9812" y1="-1.4732" x2="-1.9812" y2="-1.9812" width="0.1524" layer="21"/>
<wire x1="0.8128" y1="-2.54" x2="0.8128" y2="-2.794" width="0.1524" layer="21"/>
<wire x1="0.8128" y1="-2.794" x2="1.1938" y2="-2.794" width="0.1524" layer="21"/>
<wire x1="1.1938" y1="-2.794" x2="1.1938" y2="-2.54" width="0.1524" layer="21"/>
<wire x1="-1.1938" y1="2.54" x2="-1.1938" y2="2.794" width="0.1524" layer="21"/>
<wire x1="-1.1938" y1="2.794" x2="-0.8128" y2="2.794" width="0.1524" layer="21"/>
<wire x1="-0.8128" y1="2.794" x2="-0.8128" y2="2.54" width="0.1524" layer="21"/>
<text x="-3.5052" y="0.9906" size="1.27" layer="21" ratio="6" rot="SR0">*</text>
<wire x1="-1.9812" y1="0.7112" x2="-0.7112" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="0.8382" y1="1.9812" x2="1.143" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="1.143" y1="1.9812" x2="0.8382" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="0.3556" y1="1.9812" x2="0.6604" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="0.6604" y1="1.9812" x2="0.3556" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="-0.1524" y1="1.9812" x2="0.1524" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="0.1524" y1="1.9812" x2="-0.1524" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="-0.6604" y1="1.9812" x2="-0.3556" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="-0.3556" y1="1.9812" x2="-0.6604" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="-1.143" y1="1.9812" x2="-0.8382" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="-0.8382" y1="1.9812" x2="-1.143" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="0.8382" x2="-1.9812" y2="1.143" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="1.143" x2="-1.9812" y2="0.8382" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="0.3556" x2="-1.9812" y2="0.6604" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="0.6604" x2="-1.9812" y2="0.3556" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="-0.1524" x2="-1.9812" y2="0.1524" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="0.1524" x2="-1.9812" y2="-0.1524" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="-0.6604" x2="-1.9812" y2="-0.3556" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="-0.3556" x2="-1.9812" y2="-0.6604" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="-1.143" x2="-1.9812" y2="-0.8382" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="-0.8382" x2="-1.9812" y2="-1.143" width="0.1524" layer="51"/>
<wire x1="-0.8382" y1="-1.9812" x2="-1.143" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="-1.143" y1="-1.9812" x2="-0.8382" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="-0.3556" y1="-1.9812" x2="-0.6604" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="-0.6604" y1="-1.9812" x2="-0.3556" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="0.1524" y1="-1.9812" x2="-0.1524" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="-0.1524" y1="-1.9812" x2="0.1524" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="0.6604" y1="-1.9812" x2="0.3556" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="0.3556" y1="-1.9812" x2="0.6604" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="1.143" y1="-1.9812" x2="0.8382" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="0.8382" y1="-1.9812" x2="1.143" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="-0.8382" x2="1.9812" y2="-1.143" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="-1.143" x2="1.9812" y2="-0.8382" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="-0.3556" x2="1.9812" y2="-0.6604" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="-0.6604" x2="1.9812" y2="-0.3556" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="0.1524" x2="1.9812" y2="-0.1524" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="-0.1524" x2="1.9812" y2="0.1524" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="0.6604" x2="1.9812" y2="0.3556" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="0.3556" x2="1.9812" y2="0.6604" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="1.143" x2="1.9812" y2="0.8382" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="0.8382" x2="1.9812" y2="1.143" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="-1.9812" x2="1.9812" y2="-1.9812" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="-1.9812" x2="1.9812" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="1.9812" y1="1.9812" x2="-1.9812" y2="1.9812" width="0.1524" layer="51"/>
<wire x1="-1.9812" y1="1.9812" x2="-1.9812" y2="-1.9812" width="0.1524" layer="51"/>
<text x="-3.5052" y="0.9906" size="1.27" layer="51" ratio="6" rot="SR0">*</text>
<text x="-4.6736" y="2.667" size="2.0828" layer="25" ratio="10" rot="SR0">&gt;NAME</text>
<text x="-5.8166" y="-4.8006" size="2.0828" layer="27" ratio="10" rot="SR0">&gt;VALUE</text>
</package>
</packages>
<symbols>
<symbol name="MCP73871-2AAI/ML">
<pin name="IN_2" x="-22.86" y="10.16" length="middle" direction="in"/>
<pin name="IN" x="-22.86" y="7.62" length="middle" direction="in"/>
<pin name="VPCC" x="-22.86" y="5.08" length="middle" direction="in"/>
<pin name="*PG" x="-22.86" y="2.54" length="middle" direction="out"/>
<pin name="STAT2" x="-22.86" y="0" length="middle" direction="out"/>
<pin name="STAT1/*LBO" x="-22.86" y="-2.54" length="middle" direction="out"/>
<pin name="SEL" x="-22.86" y="-7.62" length="middle" direction="in"/>
<pin name="PROG2" x="-22.86" y="-10.16" length="middle" direction="in"/>
<pin name="*TE" x="-22.86" y="-12.7" length="middle" direction="in"/>
<pin name="CE" x="-22.86" y="-15.24" length="middle" direction="in"/>
<pin name="OUT_2" x="22.86" y="10.16" length="middle" direction="out" rot="R180"/>
<pin name="OUT" x="22.86" y="7.62" length="middle" direction="out" rot="R180"/>
<pin name="VBAT_SENSE" x="22.86" y="5.08" length="middle" rot="R180"/>
<pin name="VBAT_2" x="22.86" y="2.54" length="middle" rot="R180"/>
<pin name="VBAT" x="22.86" y="0" length="middle" rot="R180"/>
<pin name="THERM" x="22.86" y="-2.54" length="middle" rot="R180"/>
<pin name="PROG1" x="22.86" y="-5.08" length="middle" rot="R180"/>
<pin name="PROG3" x="22.86" y="-7.62" length="middle" rot="R180"/>
<pin name="VSS_2" x="22.86" y="-10.16" length="middle" direction="pwr" rot="R180"/>
<pin name="VSS" x="22.86" y="-12.7" length="middle" direction="pwr" rot="R180"/>
<pin name="EP" x="22.86" y="-15.24" length="middle" direction="pas" rot="R180"/>
<wire x1="-18.288" y1="10.16" x2="-19.3548" y2="10.668" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="10.16" x2="-19.3548" y2="9.652" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="7.62" x2="-19.3548" y2="8.128" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="7.62" x2="-19.3548" y2="7.112" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="5.08" x2="-19.3548" y2="5.588" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="5.08" x2="-19.3548" y2="4.572" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="3.048" x2="-19.3548" y2="2.54" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="2.032" x2="-19.3548" y2="2.54" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="0.508" x2="-19.3548" y2="0" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-0.508" x2="-19.3548" y2="0" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-2.032" x2="-19.3548" y2="-2.54" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-3.048" x2="-19.3548" y2="-2.54" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-7.62" x2="-19.3548" y2="-7.112" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-7.62" x2="-19.3548" y2="-8.128" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-10.16" x2="-19.3548" y2="-9.652" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-10.16" x2="-19.3548" y2="-10.668" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-12.7" x2="-19.3548" y2="-12.192" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-12.7" x2="-19.3548" y2="-13.208" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-15.24" x2="-19.3548" y2="-14.732" width="0.4064" layer="94"/>
<wire x1="-18.288" y1="-15.24" x2="-19.3548" y2="-15.748" width="0.4064" layer="94"/>
<wire x1="18.288" y1="10.668" x2="19.3548" y2="10.16" width="0.4064" layer="94"/>
<wire x1="18.288" y1="9.652" x2="19.3548" y2="10.16" width="0.4064" layer="94"/>
<wire x1="18.288" y1="8.128" x2="19.3548" y2="7.62" width="0.4064" layer="94"/>
<wire x1="18.288" y1="7.112" x2="19.3548" y2="7.62" width="0.4064" layer="94"/>
<wire x1="18.288" y1="5.08" x2="19.3548" y2="5.588" width="0.4064" layer="94"/>
<wire x1="18.288" y1="5.08" x2="19.3548" y2="4.572" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="5.588" x2="20.9042" y2="5.08" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="4.572" x2="20.9042" y2="5.08" width="0.4064" layer="94"/>
<wire x1="18.288" y1="2.54" x2="19.3548" y2="3.048" width="0.4064" layer="94"/>
<wire x1="18.288" y1="2.54" x2="19.3548" y2="2.032" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="3.048" x2="20.9042" y2="2.54" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="2.032" x2="20.9042" y2="2.54" width="0.4064" layer="94"/>
<wire x1="18.288" y1="0" x2="19.3548" y2="0.508" width="0.4064" layer="94"/>
<wire x1="18.288" y1="0" x2="19.3548" y2="-0.508" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="0.508" x2="20.9042" y2="0" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="-0.508" x2="20.9042" y2="0" width="0.4064" layer="94"/>
<wire x1="18.288" y1="-2.54" x2="19.3548" y2="-2.032" width="0.4064" layer="94"/>
<wire x1="18.288" y1="-2.54" x2="19.3548" y2="-3.048" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="-2.032" x2="20.9042" y2="-2.54" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="-3.048" x2="20.9042" y2="-2.54" width="0.4064" layer="94"/>
<wire x1="18.288" y1="-5.08" x2="19.3548" y2="-4.572" width="0.4064" layer="94"/>
<wire x1="18.288" y1="-5.08" x2="19.3548" y2="-5.588" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="-4.572" x2="20.9042" y2="-5.08" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="-5.588" x2="20.9042" y2="-5.08" width="0.4064" layer="94"/>
<wire x1="18.288" y1="-7.62" x2="19.3548" y2="-7.112" width="0.4064" layer="94"/>
<wire x1="18.288" y1="-7.62" x2="19.3548" y2="-8.128" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="-7.112" x2="20.9042" y2="-7.62" width="0.4064" layer="94"/>
<wire x1="19.8628" y1="-8.128" x2="20.9042" y2="-7.62" width="0.4064" layer="94"/>
<wire x1="-17.78" y1="15.24" x2="-17.78" y2="-20.32" width="0.4064" layer="94"/>
<wire x1="-17.78" y1="-20.32" x2="17.78" y2="-20.32" width="0.4064" layer="94"/>
<wire x1="17.78" y1="-20.32" x2="17.78" y2="15.24" width="0.4064" layer="94"/>
<wire x1="17.78" y1="15.24" x2="-17.78" y2="15.24" width="0.4064" layer="94"/>
<text x="-4.6482" y="16.7894" size="2.0828" layer="95" ratio="10" rot="SR0">&gt;NAME</text>
<text x="-5.461" y="-23.5712" size="2.0828" layer="96" ratio="10" rot="SR0">&gt;VALUE</text>
</symbol>
</symbols>
<devicesets>
<deviceset name="MCP73871-2AAI/ML" prefix="U">
<description>IC, BATTERY CHARGER, 1A</description>
<gates>
<gate name="A" symbol="MCP73871-2AAI/ML" x="0" y="0"/>
</gates>
<devices>
<device name="" package="QFN50P400X400X100-21N">
<connects>
<connect gate="A" pin="*PG" pad="6"/>
<connect gate="A" pin="*TE" pad="9"/>
<connect gate="A" pin="CE" pad="17"/>
<connect gate="A" pin="EP" pad="21"/>
<connect gate="A" pin="IN" pad="19"/>
<connect gate="A" pin="IN_2" pad="18"/>
<connect gate="A" pin="OUT" pad="20"/>
<connect gate="A" pin="OUT_2" pad="1"/>
<connect gate="A" pin="PROG1" pad="13"/>
<connect gate="A" pin="PROG2" pad="4"/>
<connect gate="A" pin="PROG3" pad="12"/>
<connect gate="A" pin="SEL" pad="3"/>
<connect gate="A" pin="STAT1/*LBO" pad="8"/>
<connect gate="A" pin="STAT2" pad="7"/>
<connect gate="A" pin="THERM" pad="5"/>
<connect gate="A" pin="VBAT" pad="14"/>
<connect gate="A" pin="VBAT_2" pad="15"/>
<connect gate="A" pin="VBAT_SENSE" pad="16"/>
<connect gate="A" pin="VPCC" pad="2"/>
<connect gate="A" pin="VSS" pad="10"/>
<connect gate="A" pin="VSS_2" pad="11"/>
</connects>
<technologies>
<technology name="">
<attribute name="MPN" value="MCP73871-2AAI/ML" constant="no"/>
<attribute name="OC_FARNELL" value="1642487" constant="no"/>
<attribute name="OC_NEWARK" value="54M4955" constant="no"/>
<attribute name="PACKAGE" value="QFN-20" constant="no"/>
<attribute name="SUPPLIER" value="Microchip" constant="no"/>
</technology>
</technologies>
</device>
</devices>
</deviceset>
</devicesets>
</library>
<library name="pinhead">
<description>&lt;b&gt;Pin Header Connectors&lt;/b&gt;&lt;p&gt;
&lt;author&gt;Created by librarian@cadsoft.de&lt;/author&gt;</description>
<packages>
<package name="1X11">
<description>&lt;b&gt;PIN HEADER&lt;/b&gt;</description>
<wire x1="9.525" y1="1.27" x2="10.795" y2="1.27" width="0.1524" layer="21"/>
<wire x1="10.795" y1="1.27" x2="11.43" y2="0.635" width="0.1524" layer="21"/>
<wire x1="11.43" y1="0.635" x2="11.43" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="11.43" y1="-0.635" x2="10.795" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="6.35" y1="0.635" x2="6.985" y2="1.27" width="0.1524" layer="21"/>
<wire x1="6.985" y1="1.27" x2="8.255" y2="1.27" width="0.1524" layer="21"/>
<wire x1="8.255" y1="1.27" x2="8.89" y2="0.635" width="0.1524" layer="21"/>
<wire x1="8.89" y1="0.635" x2="8.89" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="8.89" y1="-0.635" x2="8.255" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="8.255" y1="-1.27" x2="6.985" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="6.985" y1="-1.27" x2="6.35" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="9.525" y1="1.27" x2="8.89" y2="0.635" width="0.1524" layer="21"/>
<wire x1="8.89" y1="-0.635" x2="9.525" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="10.795" y1="-1.27" x2="9.525" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="1.905" y1="1.27" x2="3.175" y2="1.27" width="0.1524" layer="21"/>
<wire x1="3.175" y1="1.27" x2="3.81" y2="0.635" width="0.1524" layer="21"/>
<wire x1="3.81" y1="0.635" x2="3.81" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="3.81" y1="-0.635" x2="3.175" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="3.81" y1="0.635" x2="4.445" y2="1.27" width="0.1524" layer="21"/>
<wire x1="4.445" y1="1.27" x2="5.715" y2="1.27" width="0.1524" layer="21"/>
<wire x1="5.715" y1="1.27" x2="6.35" y2="0.635" width="0.1524" layer="21"/>
<wire x1="6.35" y1="0.635" x2="6.35" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="6.35" y1="-0.635" x2="5.715" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="5.715" y1="-1.27" x2="4.445" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="4.445" y1="-1.27" x2="3.81" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-1.27" y1="0.635" x2="-0.635" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-0.635" y1="1.27" x2="0.635" y2="1.27" width="0.1524" layer="21"/>
<wire x1="0.635" y1="1.27" x2="1.27" y2="0.635" width="0.1524" layer="21"/>
<wire x1="1.27" y1="0.635" x2="1.27" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="1.27" y1="-0.635" x2="0.635" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="0.635" y1="-1.27" x2="-0.635" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-0.635" y1="-1.27" x2="-1.27" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="1.905" y1="1.27" x2="1.27" y2="0.635" width="0.1524" layer="21"/>
<wire x1="1.27" y1="-0.635" x2="1.905" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="3.175" y1="-1.27" x2="1.905" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-5.715" y1="1.27" x2="-4.445" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-4.445" y1="1.27" x2="-3.81" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-3.81" y1="0.635" x2="-3.81" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-3.81" y1="-0.635" x2="-4.445" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-3.81" y1="0.635" x2="-3.175" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-3.175" y1="1.27" x2="-1.905" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-1.905" y1="1.27" x2="-1.27" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-1.27" y1="0.635" x2="-1.27" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-1.27" y1="-0.635" x2="-1.905" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-1.905" y1="-1.27" x2="-3.175" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-3.175" y1="-1.27" x2="-3.81" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-8.89" y1="0.635" x2="-8.255" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-8.255" y1="1.27" x2="-6.985" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-6.985" y1="1.27" x2="-6.35" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-6.35" y1="0.635" x2="-6.35" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-6.35" y1="-0.635" x2="-6.985" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-6.985" y1="-1.27" x2="-8.255" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-8.255" y1="-1.27" x2="-8.89" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-5.715" y1="1.27" x2="-6.35" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-6.35" y1="-0.635" x2="-5.715" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-4.445" y1="-1.27" x2="-5.715" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-13.335" y1="1.27" x2="-12.065" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-12.065" y1="1.27" x2="-11.43" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-11.43" y1="0.635" x2="-11.43" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-11.43" y1="-0.635" x2="-12.065" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-11.43" y1="0.635" x2="-10.795" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-10.795" y1="1.27" x2="-9.525" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-9.525" y1="1.27" x2="-8.89" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-8.89" y1="0.635" x2="-8.89" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-8.89" y1="-0.635" x2="-9.525" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-9.525" y1="-1.27" x2="-10.795" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-10.795" y1="-1.27" x2="-11.43" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-13.97" y1="0.635" x2="-13.97" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-13.335" y1="1.27" x2="-13.97" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-13.97" y1="-0.635" x2="-13.335" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-12.065" y1="-1.27" x2="-13.335" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="12.065" y1="1.27" x2="13.335" y2="1.27" width="0.1524" layer="21"/>
<wire x1="13.335" y1="1.27" x2="13.97" y2="0.635" width="0.1524" layer="21"/>
<wire x1="13.97" y1="0.635" x2="13.97" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="13.97" y1="-0.635" x2="13.335" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="12.065" y1="1.27" x2="11.43" y2="0.635" width="0.1524" layer="21"/>
<wire x1="11.43" y1="-0.635" x2="12.065" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="13.335" y1="-1.27" x2="12.065" y2="-1.27" width="0.1524" layer="21"/>
<pad name="1" x="-12.7" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="2" x="-10.16" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="3" x="-7.62" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="4" x="-5.08" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="5" x="-2.54" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="6" x="0" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="7" x="2.54" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="8" x="5.08" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="9" x="7.62" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="10" x="10.16" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="11" x="12.7" y="0" drill="1.016" shape="long" rot="R90"/>
<text x="-14.0462" y="1.8288" size="1.27" layer="25" ratio="10">&gt;NAME</text>
<text x="-13.97" y="-3.175" size="1.27" layer="27">&gt;VALUE</text>
<rectangle x1="9.906" y1="-0.254" x2="10.414" y2="0.254" layer="51"/>
<rectangle x1="7.366" y1="-0.254" x2="7.874" y2="0.254" layer="51"/>
<rectangle x1="4.826" y1="-0.254" x2="5.334" y2="0.254" layer="51"/>
<rectangle x1="2.286" y1="-0.254" x2="2.794" y2="0.254" layer="51"/>
<rectangle x1="-0.254" y1="-0.254" x2="0.254" y2="0.254" layer="51"/>
<rectangle x1="-2.794" y1="-0.254" x2="-2.286" y2="0.254" layer="51"/>
<rectangle x1="-5.334" y1="-0.254" x2="-4.826" y2="0.254" layer="51"/>
<rectangle x1="-7.874" y1="-0.254" x2="-7.366" y2="0.254" layer="51"/>
<rectangle x1="-10.414" y1="-0.254" x2="-9.906" y2="0.254" layer="51"/>
<rectangle x1="-12.954" y1="-0.254" x2="-12.446" y2="0.254" layer="51"/>
<rectangle x1="12.446" y1="-0.254" x2="12.954" y2="0.254" layer="51"/>
</package>
<package name="1X11/90">
<description>&lt;b&gt;PIN HEADER&lt;/b&gt;</description>
<wire x1="-13.97" y1="-1.905" x2="-11.43" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="-11.43" y1="-1.905" x2="-11.43" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-11.43" y1="0.635" x2="-13.97" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-13.97" y1="0.635" x2="-13.97" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="-12.7" y1="6.985" x2="-12.7" y2="1.27" width="0.762" layer="21"/>
<wire x1="-11.43" y1="-1.905" x2="-8.89" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="-8.89" y1="-1.905" x2="-8.89" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-8.89" y1="0.635" x2="-11.43" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-10.16" y1="6.985" x2="-10.16" y2="1.27" width="0.762" layer="21"/>
<wire x1="-8.89" y1="-1.905" x2="-6.35" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="-6.35" y1="-1.905" x2="-6.35" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-6.35" y1="0.635" x2="-8.89" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-7.62" y1="6.985" x2="-7.62" y2="1.27" width="0.762" layer="21"/>
<wire x1="-6.35" y1="-1.905" x2="-3.81" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="-3.81" y1="-1.905" x2="-3.81" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-3.81" y1="0.635" x2="-6.35" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-5.08" y1="6.985" x2="-5.08" y2="1.27" width="0.762" layer="21"/>
<wire x1="-3.81" y1="-1.905" x2="-1.27" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="-1.27" y1="-1.905" x2="-1.27" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-1.27" y1="0.635" x2="-3.81" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-2.54" y1="6.985" x2="-2.54" y2="1.27" width="0.762" layer="21"/>
<wire x1="-1.27" y1="-1.905" x2="1.27" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="1.27" y1="-1.905" x2="1.27" y2="0.635" width="0.1524" layer="21"/>
<wire x1="1.27" y1="0.635" x2="-1.27" y2="0.635" width="0.1524" layer="21"/>
<wire x1="0" y1="6.985" x2="0" y2="1.27" width="0.762" layer="21"/>
<wire x1="1.27" y1="-1.905" x2="3.81" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="3.81" y1="-1.905" x2="3.81" y2="0.635" width="0.1524" layer="21"/>
<wire x1="3.81" y1="0.635" x2="1.27" y2="0.635" width="0.1524" layer="21"/>
<wire x1="2.54" y1="6.985" x2="2.54" y2="1.27" width="0.762" layer="21"/>
<wire x1="3.81" y1="-1.905" x2="6.35" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="6.35" y1="-1.905" x2="6.35" y2="0.635" width="0.1524" layer="21"/>
<wire x1="6.35" y1="0.635" x2="3.81" y2="0.635" width="0.1524" layer="21"/>
<wire x1="5.08" y1="6.985" x2="5.08" y2="1.27" width="0.762" layer="21"/>
<wire x1="6.35" y1="-1.905" x2="8.89" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="8.89" y1="-1.905" x2="8.89" y2="0.635" width="0.1524" layer="21"/>
<wire x1="8.89" y1="0.635" x2="6.35" y2="0.635" width="0.1524" layer="21"/>
<wire x1="7.62" y1="6.985" x2="7.62" y2="1.27" width="0.762" layer="21"/>
<wire x1="8.89" y1="-1.905" x2="11.43" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="11.43" y1="-1.905" x2="11.43" y2="0.635" width="0.1524" layer="21"/>
<wire x1="11.43" y1="0.635" x2="8.89" y2="0.635" width="0.1524" layer="21"/>
<wire x1="10.16" y1="6.985" x2="10.16" y2="1.27" width="0.762" layer="21"/>
<wire x1="11.43" y1="-1.905" x2="13.97" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="13.97" y1="-1.905" x2="13.97" y2="0.635" width="0.1524" layer="21"/>
<wire x1="13.97" y1="0.635" x2="11.43" y2="0.635" width="0.1524" layer="21"/>
<wire x1="12.7" y1="6.985" x2="12.7" y2="1.27" width="0.762" layer="21"/>
<pad name="1" x="-12.7" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="2" x="-10.16" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="3" x="-7.62" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="4" x="-5.08" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="5" x="-2.54" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="6" x="0" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="7" x="2.54" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="8" x="5.08" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="9" x="7.62" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="10" x="10.16" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="11" x="12.7" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<text x="-14.605" y="-3.81" size="1.27" layer="25" ratio="10" rot="R90">&gt;NAME</text>
<text x="15.875" y="-4.445" size="1.27" layer="27" rot="R90">&gt;VALUE</text>
<rectangle x1="-13.081" y1="0.635" x2="-12.319" y2="1.143" layer="21"/>
<rectangle x1="-10.541" y1="0.635" x2="-9.779" y2="1.143" layer="21"/>
<rectangle x1="-8.001" y1="0.635" x2="-7.239" y2="1.143" layer="21"/>
<rectangle x1="-5.461" y1="0.635" x2="-4.699" y2="1.143" layer="21"/>
<rectangle x1="-2.921" y1="0.635" x2="-2.159" y2="1.143" layer="21"/>
<rectangle x1="-0.381" y1="0.635" x2="0.381" y2="1.143" layer="21"/>
<rectangle x1="2.159" y1="0.635" x2="2.921" y2="1.143" layer="21"/>
<rectangle x1="4.699" y1="0.635" x2="5.461" y2="1.143" layer="21"/>
<rectangle x1="7.239" y1="0.635" x2="8.001" y2="1.143" layer="21"/>
<rectangle x1="9.779" y1="0.635" x2="10.541" y2="1.143" layer="21"/>
<rectangle x1="12.319" y1="0.635" x2="13.081" y2="1.143" layer="21"/>
<rectangle x1="-13.081" y1="-2.921" x2="-12.319" y2="-1.905" layer="21"/>
<rectangle x1="-10.541" y1="-2.921" x2="-9.779" y2="-1.905" layer="21"/>
<rectangle x1="-8.001" y1="-2.921" x2="-7.239" y2="-1.905" layer="21"/>
<rectangle x1="-5.461" y1="-2.921" x2="-4.699" y2="-1.905" layer="21"/>
<rectangle x1="-2.921" y1="-2.921" x2="-2.159" y2="-1.905" layer="21"/>
<rectangle x1="-0.381" y1="-2.921" x2="0.381" y2="-1.905" layer="21"/>
<rectangle x1="2.159" y1="-2.921" x2="2.921" y2="-1.905" layer="21"/>
<rectangle x1="4.699" y1="-2.921" x2="5.461" y2="-1.905" layer="21"/>
<rectangle x1="7.239" y1="-2.921" x2="8.001" y2="-1.905" layer="21"/>
<rectangle x1="9.779" y1="-2.921" x2="10.541" y2="-1.905" layer="21"/>
<rectangle x1="12.319" y1="-2.921" x2="13.081" y2="-1.905" layer="21"/>
</package>
</packages>
<symbols>
<symbol name="PINHD11">
<wire x1="-6.35" y1="-15.24" x2="1.27" y2="-15.24" width="0.4064" layer="94"/>
<wire x1="1.27" y1="-15.24" x2="1.27" y2="15.24" width="0.4064" layer="94"/>
<wire x1="1.27" y1="15.24" x2="-6.35" y2="15.24" width="0.4064" layer="94"/>
<wire x1="-6.35" y1="15.24" x2="-6.35" y2="-15.24" width="0.4064" layer="94"/>
<text x="-6.35" y="15.875" size="1.778" layer="95">&gt;NAME</text>
<text x="-6.35" y="-17.78" size="1.778" layer="96">&gt;VALUE</text>
<pin name="1" x="-2.54" y="12.7" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="2" x="-2.54" y="10.16" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="3" x="-2.54" y="7.62" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="4" x="-2.54" y="5.08" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="5" x="-2.54" y="2.54" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="6" x="-2.54" y="0" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="7" x="-2.54" y="-2.54" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="8" x="-2.54" y="-5.08" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="9" x="-2.54" y="-7.62" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="10" x="-2.54" y="-10.16" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="11" x="-2.54" y="-12.7" visible="pad" length="short" direction="pas" function="dot"/>
</symbol>
</symbols>
<devicesets>
<deviceset name="PINHD-1X11" prefix="JP" uservalue="yes">
<description>&lt;b&gt;PIN HEADER&lt;/b&gt;</description>
<gates>
<gate name="A" symbol="PINHD11" x="0" y="0"/>
</gates>
<devices>
<device name="" package="1X11">
<connects>
<connect gate="A" pin="1" pad="1"/>
<connect gate="A" pin="10" pad="10"/>
<connect gate="A" pin="11" pad="11"/>
<connect gate="A" pin="2" pad="2"/>
<connect gate="A" pin="3" pad="3"/>
<connect gate="A" pin="4" pad="4"/>
<connect gate="A" pin="5" pad="5"/>
<connect gate="A" pin="6" pad="6"/>
<connect gate="A" pin="7" pad="7"/>
<connect gate="A" pin="8" pad="8"/>
<connect gate="A" pin="9" pad="9"/>
</connects>
<technologies>
<technology name=""/>
</technologies>
</device>
<device name="/90" package="1X11/90">
<connects>
<connect gate="A" pin="1" pad="1"/>
<connect gate="A" pin="10" pad="10"/>
<connect gate="A" pin="11" pad="11"/>
<connect gate="A" pin="2" pad="2"/>
<connect gate="A" pin="3" pad="3"/>
<connect gate="A" pin="4" pad="4"/>
<connect gate="A" pin="5" pad="5"/>
<connect gate="A" pin="6" pad="6"/>
<connect gate="A" pin="7" pad="7"/>
<connect gate="A" pin="8" pad="8"/>
<connect gate="A" pin="9" pad="9"/>
</connects>
<technologies>
<technology name=""/>
</technologies>
</device>
</devices>
</deviceset>
</devicesets>
</library>
</libraries>
<attributes>
</attributes>
<variantdefs>
</variantdefs>
<classes>
<class number="0" name="default" width="0" drill="0">
</class>
</classes>
<parts>
<part name="U1" library="Microchip-MCP73871-2AAI_ML" deviceset="MCP73871-2AAI/ML" device=""/>
<part name="JP1" library="pinhead" deviceset="PINHD-1X11" device=""/>
<part name="JP2" library="pinhead" deviceset="PINHD-1X11" device=""/>
</parts>
<sheets>
<sheet>
<plain>
</plain>
<instances>
<instance part="U1" gate="A" x="86.36" y="50.8"/>
<instance part="JP1" gate="A" x="154.94" y="55.88"/>
<instance part="JP2" gate="A" x="5.08" y="48.26" rot="MR0"/>
</instances>
<busses>
</busses>
<nets>
<net name="N$1" class="0">
<segment>
<pinref part="JP2" gate="A" pin="1"/>
<wire x1="7.62" y1="60.96" x2="25.4" y2="60.96" width="0.1524" layer="91"/>
<wire x1="25.4" y1="60.96" x2="25.4" y2="68.58" width="0.1524" layer="91"/>
<wire x1="25.4" y1="68.58" x2="111.76" y2="68.58" width="0.1524" layer="91"/>
<wire x1="111.76" y1="68.58" x2="111.76" y2="60.96" width="0.1524" layer="91"/>
<pinref part="U1" gate="A" pin="OUT_2"/>
<wire x1="111.76" y1="60.96" x2="109.22" y2="60.96" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$2" class="0">
<segment>
<pinref part="U1" gate="A" pin="VPCC"/>
<wire x1="63.5" y1="55.88" x2="25.4" y2="55.88" width="0.1524" layer="91"/>
<wire x1="25.4" y1="55.88" x2="25.4" y2="58.42" width="0.1524" layer="91"/>
<pinref part="JP2" gate="A" pin="2"/>
<wire x1="25.4" y1="58.42" x2="7.62" y2="58.42" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$3" class="0">
<segment>
<pinref part="JP2" gate="A" pin="3"/>
<wire x1="7.62" y1="55.88" x2="22.86" y2="55.88" width="0.1524" layer="91"/>
<wire x1="22.86" y1="55.88" x2="22.86" y2="53.34" width="0.1524" layer="91"/>
<wire x1="22.86" y1="53.34" x2="40.64" y2="53.34" width="0.1524" layer="91"/>
<wire x1="40.64" y1="53.34" x2="40.64" y2="43.18" width="0.1524" layer="91"/>
<pinref part="U1" gate="A" pin="SEL"/>
<wire x1="40.64" y1="43.18" x2="63.5" y2="43.18" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$4" class="0">
<segment>
<pinref part="U1" gate="A" pin="PROG2"/>
<wire x1="63.5" y1="40.64" x2="38.1" y2="40.64" width="0.1524" layer="91"/>
<wire x1="38.1" y1="40.64" x2="38.1" y2="50.8" width="0.1524" layer="91"/>
<wire x1="38.1" y1="50.8" x2="20.32" y2="50.8" width="0.1524" layer="91"/>
<wire x1="20.32" y1="50.8" x2="20.32" y2="53.34" width="0.1524" layer="91"/>
<pinref part="JP2" gate="A" pin="4"/>
<wire x1="20.32" y1="53.34" x2="7.62" y2="53.34" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$5" class="0">
<segment>
<pinref part="JP2" gate="A" pin="5"/>
<wire x1="7.62" y1="50.8" x2="17.78" y2="50.8" width="0.1524" layer="91"/>
<wire x1="17.78" y1="50.8" x2="17.78" y2="71.12" width="0.1524" layer="91"/>
<wire x1="17.78" y1="71.12" x2="119.38" y2="71.12" width="0.1524" layer="91"/>
<wire x1="119.38" y1="71.12" x2="119.38" y2="48.26" width="0.1524" layer="91"/>
<pinref part="U1" gate="A" pin="THERM"/>
<wire x1="119.38" y1="48.26" x2="109.22" y2="48.26" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$6" class="0">
<segment>
<pinref part="U1" gate="A" pin="*PG"/>
<wire x1="63.5" y1="53.34" x2="43.18" y2="53.34" width="0.1524" layer="91"/>
<wire x1="43.18" y1="53.34" x2="43.18" y2="48.26" width="0.1524" layer="91"/>
<pinref part="JP2" gate="A" pin="6"/>
<wire x1="43.18" y1="48.26" x2="7.62" y2="48.26" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$7" class="0">
<segment>
<pinref part="JP2" gate="A" pin="7"/>
<wire x1="7.62" y1="45.72" x2="45.72" y2="45.72" width="0.1524" layer="91"/>
<wire x1="45.72" y1="45.72" x2="45.72" y2="50.8" width="0.1524" layer="91"/>
<pinref part="U1" gate="A" pin="STAT2"/>
<wire x1="45.72" y1="50.8" x2="63.5" y2="50.8" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$8" class="0">
<segment>
<pinref part="U1" gate="A" pin="STAT1/*LBO"/>
<wire x1="63.5" y1="48.26" x2="48.26" y2="48.26" width="0.1524" layer="91"/>
<wire x1="48.26" y1="48.26" x2="48.26" y2="38.1" width="0.1524" layer="91"/>
<wire x1="48.26" y1="38.1" x2="35.56" y2="38.1" width="0.1524" layer="91"/>
<wire x1="35.56" y1="38.1" x2="35.56" y2="43.18" width="0.1524" layer="91"/>
<pinref part="JP2" gate="A" pin="8"/>
<wire x1="35.56" y1="43.18" x2="7.62" y2="43.18" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$9" class="0">
<segment>
<pinref part="JP2" gate="A" pin="9"/>
<wire x1="7.62" y1="40.64" x2="33.02" y2="40.64" width="0.1524" layer="91"/>
<wire x1="33.02" y1="40.64" x2="33.02" y2="38.1" width="0.1524" layer="91"/>
<wire x1="33.02" y1="38.1" x2="33.02" y2="35.56" width="0.1524" layer="91"/>
<wire x1="33.02" y1="35.56" x2="50.8" y2="35.56" width="0.1524" layer="91"/>
<wire x1="50.8" y1="35.56" x2="50.8" y2="38.1" width="0.1524" layer="91"/>
<pinref part="U1" gate="A" pin="*TE"/>
<wire x1="50.8" y1="38.1" x2="63.5" y2="38.1" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$10" class="0">
<segment>
<pinref part="U1" gate="A" pin="VSS"/>
<wire x1="109.22" y1="38.1" x2="111.76" y2="38.1" width="0.1524" layer="91"/>
<wire x1="111.76" y1="38.1" x2="111.76" y2="27.94" width="0.1524" layer="91"/>
<wire x1="111.76" y1="27.94" x2="15.24" y2="27.94" width="0.1524" layer="91"/>
<wire x1="15.24" y1="27.94" x2="15.24" y2="38.1" width="0.1524" layer="91"/>
<pinref part="JP2" gate="A" pin="10"/>
<wire x1="15.24" y1="38.1" x2="7.62" y2="38.1" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$11" class="0">
<segment>
<pinref part="U1" gate="A" pin="VSS_2"/>
<wire x1="109.22" y1="40.64" x2="116.84" y2="40.64" width="0.1524" layer="91"/>
<wire x1="116.84" y1="40.64" x2="116.84" y2="38.1" width="0.1524" layer="91"/>
<pinref part="JP2" gate="A" pin="11"/>
<wire x1="7.62" y1="35.56" x2="17.78" y2="35.56" width="0.1524" layer="91"/>
<wire x1="17.78" y1="35.56" x2="17.78" y2="22.86" width="0.1524" layer="91"/>
<wire x1="17.78" y1="22.86" x2="116.84" y2="22.86" width="0.1524" layer="91"/>
<wire x1="116.84" y1="22.86" x2="116.84" y2="38.1" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$12" class="0">
<segment>
<pinref part="U1" gate="A" pin="PROG3"/>
<wire x1="109.22" y1="43.18" x2="119.38" y2="43.18" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="11"/>
<wire x1="119.38" y1="43.18" x2="152.4" y2="43.18" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$13" class="0">
<segment>
<pinref part="U1" gate="A" pin="PROG1"/>
<wire x1="109.22" y1="45.72" x2="121.92" y2="45.72" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="10"/>
<wire x1="121.92" y1="45.72" x2="152.4" y2="45.72" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$14" class="0">
<segment>
<pinref part="U1" gate="A" pin="VBAT"/>
<wire x1="109.22" y1="50.8" x2="124.46" y2="50.8" width="0.1524" layer="91"/>
<wire x1="124.46" y1="50.8" x2="124.46" y2="48.26" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="9"/>
<wire x1="124.46" y1="48.26" x2="152.4" y2="48.26" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$15" class="0">
<segment>
<pinref part="U1" gate="A" pin="VBAT_2"/>
<wire x1="109.22" y1="53.34" x2="127" y2="53.34" width="0.1524" layer="91"/>
<wire x1="127" y1="53.34" x2="127" y2="50.8" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="8"/>
<wire x1="127" y1="50.8" x2="152.4" y2="50.8" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$16" class="0">
<segment>
<pinref part="U1" gate="A" pin="VBAT_SENSE"/>
<wire x1="129.54" y1="55.88" x2="109.22" y2="55.88" width="0.1524" layer="91"/>
<wire x1="129.54" y1="55.88" x2="129.54" y2="53.34" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="7"/>
<wire x1="129.54" y1="53.34" x2="152.4" y2="53.34" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$17" class="0">
<segment>
<pinref part="U1" gate="A" pin="CE"/>
<wire x1="63.5" y1="35.56" x2="63.5" y2="25.4" width="0.1524" layer="91"/>
<wire x1="63.5" y1="25.4" x2="132.08" y2="25.4" width="0.1524" layer="91"/>
<wire x1="132.08" y1="25.4" x2="132.08" y2="55.88" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="6"/>
<wire x1="132.08" y1="55.88" x2="152.4" y2="55.88" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$18" class="0">
<segment>
<pinref part="U1" gate="A" pin="IN_2"/>
<wire x1="63.5" y1="60.96" x2="63.5" y2="73.66" width="0.1524" layer="91"/>
<wire x1="63.5" y1="73.66" x2="121.92" y2="73.66" width="0.1524" layer="91"/>
<wire x1="121.92" y1="73.66" x2="121.92" y2="58.42" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="5"/>
<wire x1="121.92" y1="58.42" x2="152.4" y2="58.42" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$19" class="0">
<segment>
<pinref part="U1" gate="A" pin="IN"/>
<wire x1="63.5" y1="58.42" x2="60.96" y2="58.42" width="0.1524" layer="91"/>
<wire x1="60.96" y1="58.42" x2="60.96" y2="76.2" width="0.1524" layer="91"/>
<wire x1="60.96" y1="76.2" x2="124.46" y2="76.2" width="0.1524" layer="91"/>
<wire x1="124.46" y1="76.2" x2="124.46" y2="60.96" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="4"/>
<wire x1="124.46" y1="60.96" x2="152.4" y2="60.96" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$20" class="0">
<segment>
<pinref part="U1" gate="A" pin="OUT"/>
<wire x1="109.22" y1="58.42" x2="116.84" y2="58.42" width="0.1524" layer="91"/>
<wire x1="116.84" y1="58.42" x2="116.84" y2="63.5" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="3"/>
<wire x1="116.84" y1="63.5" x2="152.4" y2="63.5" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$21" class="0">
<segment>
<pinref part="U1" gate="A" pin="EP"/>
<wire x1="109.22" y1="35.56" x2="134.62" y2="35.56" width="0.1524" layer="91"/>
<wire x1="134.62" y1="35.56" x2="134.62" y2="38.1" width="0.1524" layer="91"/>
<wire x1="134.62" y1="38.1" x2="134.62" y2="66.04" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="2"/>
<wire x1="134.62" y1="66.04" x2="152.4" y2="66.04" width="0.1524" layer="91"/>
</segment>
</net>
</nets>
</sheet>
</sheets>
</schematic>
</drawing>
</eagle>
