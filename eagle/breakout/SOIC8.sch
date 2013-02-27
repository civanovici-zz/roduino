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
<library name="pinhead">
<description>&lt;b&gt;Pin Header Connectors&lt;/b&gt;&lt;p&gt;
&lt;author&gt;Created by librarian@cadsoft.de&lt;/author&gt;</description>
<packages>
<package name="1X04">
<description>&lt;b&gt;PIN HEADER&lt;/b&gt;</description>
<wire x1="0" y1="0.635" x2="0.635" y2="1.27" width="0.1524" layer="21"/>
<wire x1="0.635" y1="1.27" x2="1.905" y2="1.27" width="0.1524" layer="21"/>
<wire x1="1.905" y1="1.27" x2="2.54" y2="0.635" width="0.1524" layer="21"/>
<wire x1="2.54" y1="0.635" x2="2.54" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="2.54" y1="-0.635" x2="1.905" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="1.905" y1="-1.27" x2="0.635" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="0.635" y1="-1.27" x2="0" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-4.445" y1="1.27" x2="-3.175" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-3.175" y1="1.27" x2="-2.54" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-2.54" y1="0.635" x2="-2.54" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-2.54" y1="-0.635" x2="-3.175" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-2.54" y1="0.635" x2="-1.905" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-1.905" y1="1.27" x2="-0.635" y2="1.27" width="0.1524" layer="21"/>
<wire x1="-0.635" y1="1.27" x2="0" y2="0.635" width="0.1524" layer="21"/>
<wire x1="0" y1="0.635" x2="0" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="0" y1="-0.635" x2="-0.635" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-0.635" y1="-1.27" x2="-1.905" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-1.905" y1="-1.27" x2="-2.54" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-5.08" y1="0.635" x2="-5.08" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="-4.445" y1="1.27" x2="-5.08" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-5.08" y1="-0.635" x2="-4.445" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="-3.175" y1="-1.27" x2="-4.445" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="3.175" y1="1.27" x2="4.445" y2="1.27" width="0.1524" layer="21"/>
<wire x1="4.445" y1="1.27" x2="5.08" y2="0.635" width="0.1524" layer="21"/>
<wire x1="5.08" y1="0.635" x2="5.08" y2="-0.635" width="0.1524" layer="21"/>
<wire x1="5.08" y1="-0.635" x2="4.445" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="3.175" y1="1.27" x2="2.54" y2="0.635" width="0.1524" layer="21"/>
<wire x1="2.54" y1="-0.635" x2="3.175" y2="-1.27" width="0.1524" layer="21"/>
<wire x1="4.445" y1="-1.27" x2="3.175" y2="-1.27" width="0.1524" layer="21"/>
<pad name="1" x="-3.81" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="2" x="-1.27" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="3" x="1.27" y="0" drill="1.016" shape="long" rot="R90"/>
<pad name="4" x="3.81" y="0" drill="1.016" shape="long" rot="R90"/>
<text x="-5.1562" y="1.8288" size="1.27" layer="25" ratio="10">&gt;NAME</text>
<text x="-5.08" y="-3.175" size="1.27" layer="27">&gt;VALUE</text>
<rectangle x1="1.016" y1="-0.254" x2="1.524" y2="0.254" layer="51"/>
<rectangle x1="-1.524" y1="-0.254" x2="-1.016" y2="0.254" layer="51"/>
<rectangle x1="-4.064" y1="-0.254" x2="-3.556" y2="0.254" layer="51"/>
<rectangle x1="3.556" y1="-0.254" x2="4.064" y2="0.254" layer="51"/>
</package>
<package name="1X04/90">
<description>&lt;b&gt;PIN HEADER&lt;/b&gt;</description>
<wire x1="-5.08" y1="-1.905" x2="-2.54" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="-2.54" y1="-1.905" x2="-2.54" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-2.54" y1="0.635" x2="-5.08" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-5.08" y1="0.635" x2="-5.08" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="-3.81" y1="6.985" x2="-3.81" y2="1.27" width="0.762" layer="21"/>
<wire x1="-2.54" y1="-1.905" x2="0" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="0" y1="-1.905" x2="0" y2="0.635" width="0.1524" layer="21"/>
<wire x1="0" y1="0.635" x2="-2.54" y2="0.635" width="0.1524" layer="21"/>
<wire x1="-1.27" y1="6.985" x2="-1.27" y2="1.27" width="0.762" layer="21"/>
<wire x1="0" y1="-1.905" x2="2.54" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="2.54" y1="-1.905" x2="2.54" y2="0.635" width="0.1524" layer="21"/>
<wire x1="2.54" y1="0.635" x2="0" y2="0.635" width="0.1524" layer="21"/>
<wire x1="1.27" y1="6.985" x2="1.27" y2="1.27" width="0.762" layer="21"/>
<wire x1="2.54" y1="-1.905" x2="5.08" y2="-1.905" width="0.1524" layer="21"/>
<wire x1="5.08" y1="-1.905" x2="5.08" y2="0.635" width="0.1524" layer="21"/>
<wire x1="5.08" y1="0.635" x2="2.54" y2="0.635" width="0.1524" layer="21"/>
<wire x1="3.81" y1="6.985" x2="3.81" y2="1.27" width="0.762" layer="21"/>
<pad name="1" x="-3.81" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="2" x="-1.27" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="3" x="1.27" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<pad name="4" x="3.81" y="-3.81" drill="1.016" shape="long" rot="R90"/>
<text x="-5.715" y="-3.81" size="1.27" layer="25" ratio="10" rot="R90">&gt;NAME</text>
<text x="6.985" y="-4.445" size="1.27" layer="27" rot="R90">&gt;VALUE</text>
<rectangle x1="-4.191" y1="0.635" x2="-3.429" y2="1.143" layer="21"/>
<rectangle x1="-1.651" y1="0.635" x2="-0.889" y2="1.143" layer="21"/>
<rectangle x1="0.889" y1="0.635" x2="1.651" y2="1.143" layer="21"/>
<rectangle x1="3.429" y1="0.635" x2="4.191" y2="1.143" layer="21"/>
<rectangle x1="-4.191" y1="-2.921" x2="-3.429" y2="-1.905" layer="21"/>
<rectangle x1="-1.651" y1="-2.921" x2="-0.889" y2="-1.905" layer="21"/>
<rectangle x1="0.889" y1="-2.921" x2="1.651" y2="-1.905" layer="21"/>
<rectangle x1="3.429" y1="-2.921" x2="4.191" y2="-1.905" layer="21"/>
</package>
</packages>
<symbols>
<symbol name="PINHD4">
<wire x1="-6.35" y1="-5.08" x2="1.27" y2="-5.08" width="0.4064" layer="94"/>
<wire x1="1.27" y1="-5.08" x2="1.27" y2="7.62" width="0.4064" layer="94"/>
<wire x1="1.27" y1="7.62" x2="-6.35" y2="7.62" width="0.4064" layer="94"/>
<wire x1="-6.35" y1="7.62" x2="-6.35" y2="-5.08" width="0.4064" layer="94"/>
<text x="-6.35" y="8.255" size="1.778" layer="95">&gt;NAME</text>
<text x="-6.35" y="-7.62" size="1.778" layer="96">&gt;VALUE</text>
<pin name="1" x="-2.54" y="5.08" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="2" x="-2.54" y="2.54" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="3" x="-2.54" y="0" visible="pad" length="short" direction="pas" function="dot"/>
<pin name="4" x="-2.54" y="-2.54" visible="pad" length="short" direction="pas" function="dot"/>
</symbol>
</symbols>
<devicesets>
<deviceset name="PINHD-1X4" prefix="JP" uservalue="yes">
<description>&lt;b&gt;PIN HEADER&lt;/b&gt;</description>
<gates>
<gate name="A" symbol="PINHD4" x="0" y="0"/>
</gates>
<devices>
<device name="" package="1X04">
<connects>
<connect gate="A" pin="1" pad="1"/>
<connect gate="A" pin="2" pad="2"/>
<connect gate="A" pin="3" pad="3"/>
<connect gate="A" pin="4" pad="4"/>
</connects>
<technologies>
<technology name=""/>
</technologies>
</device>
<device name="/90" package="1X04/90">
<connects>
<connect gate="A" pin="1" pad="1"/>
<connect gate="A" pin="2" pad="2"/>
<connect gate="A" pin="3" pad="3"/>
<connect gate="A" pin="4" pad="4"/>
</connects>
<technologies>
<technology name=""/>
</technologies>
</device>
</devices>
</deviceset>
</devicesets>
</library>
<library name="Atmel-ATTINY13-20SU">
<packages>
<package name="SOIC127P798X216-8N">
<smd name="1" x="-3.7084" y="1.905" dx="1.5494" dy="0.5334" layer="1"/>
<smd name="2" x="-3.7084" y="0.635" dx="1.5494" dy="0.5334" layer="1"/>
<smd name="3" x="-3.7084" y="-0.635" dx="1.5494" dy="0.5334" layer="1"/>
<smd name="4" x="-3.7084" y="-1.905" dx="1.5494" dy="0.5334" layer="1"/>
<smd name="5" x="3.7084" y="-1.905" dx="1.5494" dy="0.5334" layer="1"/>
<smd name="6" x="3.7084" y="-0.635" dx="1.5494" dy="0.5334" layer="1"/>
<smd name="7" x="3.7084" y="0.635" dx="1.5494" dy="0.5334" layer="1"/>
<smd name="8" x="3.7084" y="1.905" dx="1.5494" dy="0.5334" layer="1"/>
<wire x1="-2.6924" y1="1.6764" x2="-2.6924" y2="2.1336" width="0" layer="51"/>
<wire x1="-2.6924" y1="2.1336" x2="-4.1402" y2="2.1336" width="0" layer="51"/>
<wire x1="-4.1402" y1="2.1336" x2="-4.1402" y2="1.6764" width="0" layer="51"/>
<wire x1="-4.1402" y1="1.6764" x2="-2.6924" y2="1.6764" width="0" layer="51"/>
<wire x1="-2.6924" y1="0.4064" x2="-2.6924" y2="0.8636" width="0" layer="51"/>
<wire x1="-2.6924" y1="0.8636" x2="-4.1402" y2="0.8636" width="0" layer="51"/>
<wire x1="-4.1402" y1="0.8636" x2="-4.1402" y2="0.4064" width="0" layer="51"/>
<wire x1="-4.1402" y1="0.4064" x2="-2.6924" y2="0.4064" width="0" layer="51"/>
<wire x1="-2.6924" y1="-0.8636" x2="-2.6924" y2="-0.4064" width="0" layer="51"/>
<wire x1="-2.6924" y1="-0.4064" x2="-4.1402" y2="-0.4064" width="0" layer="51"/>
<wire x1="-4.1402" y1="-0.4064" x2="-4.1402" y2="-0.8636" width="0" layer="51"/>
<wire x1="-4.1402" y1="-0.8636" x2="-2.6924" y2="-0.8636" width="0" layer="51"/>
<wire x1="-2.6924" y1="-2.1336" x2="-2.6924" y2="-1.6764" width="0" layer="51"/>
<wire x1="-2.6924" y1="-1.6764" x2="-4.1402" y2="-1.6764" width="0" layer="51"/>
<wire x1="-4.1402" y1="-1.6764" x2="-4.1402" y2="-2.1336" width="0" layer="51"/>
<wire x1="-4.1402" y1="-2.1336" x2="-2.6924" y2="-2.1336" width="0" layer="51"/>
<wire x1="2.6924" y1="-1.6764" x2="2.6924" y2="-2.1336" width="0" layer="51"/>
<wire x1="2.6924" y1="-2.1336" x2="4.1402" y2="-2.1336" width="0" layer="51"/>
<wire x1="4.1402" y1="-2.1336" x2="4.1402" y2="-1.6764" width="0" layer="51"/>
<wire x1="4.1402" y1="-1.6764" x2="2.6924" y2="-1.6764" width="0" layer="51"/>
<wire x1="2.6924" y1="-0.4064" x2="2.6924" y2="-0.8636" width="0" layer="51"/>
<wire x1="2.6924" y1="-0.8636" x2="4.1402" y2="-0.8636" width="0" layer="51"/>
<wire x1="4.1402" y1="-0.8636" x2="4.1402" y2="-0.4064" width="0" layer="51"/>
<wire x1="4.1402" y1="-0.4064" x2="2.6924" y2="-0.4064" width="0" layer="51"/>
<wire x1="2.6924" y1="0.8636" x2="2.6924" y2="0.4064" width="0" layer="51"/>
<wire x1="2.6924" y1="0.4064" x2="4.1402" y2="0.4064" width="0" layer="51"/>
<wire x1="4.1402" y1="0.4064" x2="4.1402" y2="0.8636" width="0" layer="51"/>
<wire x1="4.1402" y1="0.8636" x2="2.6924" y2="0.8636" width="0" layer="51"/>
<wire x1="2.6924" y1="2.1336" x2="2.6924" y2="1.6764" width="0" layer="51"/>
<wire x1="2.6924" y1="1.6764" x2="4.1402" y2="1.6764" width="0" layer="51"/>
<wire x1="4.1402" y1="1.6764" x2="4.1402" y2="2.1336" width="0" layer="51"/>
<wire x1="4.1402" y1="2.1336" x2="2.6924" y2="2.1336" width="0" layer="51"/>
<wire x1="-2.6924" y1="-2.667" x2="2.6924" y2="-2.667" width="0" layer="51"/>
<wire x1="2.6924" y1="-2.667" x2="2.6924" y2="2.667" width="0" layer="51"/>
<wire x1="2.6924" y1="2.667" x2="-2.6924" y2="2.667" width="0" layer="51"/>
<wire x1="-2.6924" y1="2.667" x2="-2.6924" y2="-2.667" width="0" layer="51"/>
<wire x1="0.3048" y1="2.667" x2="-0.3048" y2="2.667" width="0" layer="51" curve="-180"/>
<text x="-4.5466" y="2.3114" size="1.27" layer="51" ratio="6" rot="SR0">*</text>
<wire x1="2.6924" y1="2.413" x2="2.6924" y2="2.667" width="0.1524" layer="21"/>
<wire x1="2.6924" y1="1.143" x2="2.6924" y2="1.397" width="0.1524" layer="21"/>
<wire x1="2.6924" y1="-0.127" x2="2.6924" y2="0.127" width="0.1524" layer="21"/>
<wire x1="2.6924" y1="-1.397" x2="2.6924" y2="-1.143" width="0.1524" layer="21"/>
<wire x1="-2.6924" y1="-2.413" x2="-2.6924" y2="-2.667" width="0.1524" layer="21"/>
<wire x1="-2.6924" y1="-1.143" x2="-2.6924" y2="-1.397" width="0.1524" layer="21"/>
<wire x1="-2.6924" y1="0.127" x2="-2.6924" y2="-0.127" width="0.1524" layer="21"/>
<wire x1="-2.6924" y1="-2.667" x2="2.6924" y2="-2.667" width="0.1524" layer="21"/>
<wire x1="2.6924" y1="-2.667" x2="2.6924" y2="-2.413" width="0.1524" layer="21"/>
<wire x1="2.6924" y1="2.667" x2="-2.6924" y2="2.667" width="0.1524" layer="21"/>
<wire x1="-2.6924" y1="2.667" x2="-2.6924" y2="2.413" width="0.1524" layer="21"/>
<wire x1="-2.6924" y1="1.397" x2="-2.6924" y2="1.143" width="0.1524" layer="21"/>
<wire x1="0.3048" y1="2.667" x2="-0.3048" y2="2.667" width="0.1524" layer="21" curve="-180"/>
<text x="-4.5466" y="2.3114" size="1.27" layer="21" ratio="6" rot="SR0">*</text>
<text x="-4.6228" y="-5.461" size="2.0828" layer="25" ratio="10" rot="SR0">&gt;NAME</text>
<text x="-3.4544" y="-0.635" size="1.27" layer="27" ratio="6" rot="SR0">&gt;VALUE</text>
</package>
</packages>
<symbols>
<symbol name="ATTINY13-20SU">
<pin name="VCC" x="-17.78" y="2.54" length="middle" direction="pwr"/>
<pin name="GND" x="-17.78" y="-7.62" length="middle" direction="pas"/>
<pin name="PB0" x="17.78" y="2.54" length="middle" rot="R180"/>
<pin name="PB1" x="17.78" y="0" length="middle" rot="R180"/>
<pin name="PB2" x="17.78" y="-2.54" length="middle" rot="R180"/>
<pin name="PB3" x="17.78" y="-5.08" length="middle" rot="R180"/>
<pin name="PB4" x="17.78" y="-7.62" length="middle" rot="R180"/>
<pin name="PB5" x="17.78" y="-10.16" length="middle" rot="R180"/>
<wire x1="-12.7" y1="7.62" x2="-12.7" y2="-15.24" width="0.4064" layer="94"/>
<wire x1="-12.7" y1="-15.24" x2="12.7" y2="-15.24" width="0.4064" layer="94"/>
<wire x1="12.7" y1="-15.24" x2="12.7" y2="7.62" width="0.4064" layer="94"/>
<wire x1="12.7" y1="7.62" x2="-12.7" y2="7.62" width="0.4064" layer="94"/>
<text x="-5.5118" y="10.2108" size="2.0828" layer="95" ratio="10" rot="SR0">RefDes</text>
<text x="-4.4704" y="-19.8628" size="2.0828" layer="96" ratio="10" rot="SR0">Type</text>
</symbol>
</symbols>
<devicesets>
<deviceset name="ATTINY13-20SU" prefix="U">
<gates>
<gate name="A" symbol="ATTINY13-20SU" x="0" y="0"/>
</gates>
<devices>
<device name="" package="SOIC127P798X216-8N">
<connects>
<connect gate="A" pin="GND" pad="4"/>
<connect gate="A" pin="PB0" pad="5"/>
<connect gate="A" pin="PB1" pad="6"/>
<connect gate="A" pin="PB2" pad="7"/>
<connect gate="A" pin="PB3" pad="2"/>
<connect gate="A" pin="PB4" pad="3"/>
<connect gate="A" pin="PB5" pad="1"/>
<connect gate="A" pin="VCC" pad="8"/>
</connects>
<technologies>
<technology name="">
<attribute name="DESCRIPTION" value="8-bit Microcontroller with In-System Programmable Flash" constant="no"/>
<attribute name="MPN" value="ATTINY13-20SU" constant="no"/>
<attribute name="OC_FARNELL" value="9171568" constant="no"/>
<attribute name="OC_NEWARK" value="96K6522" constant="no"/>
<attribute name="PACKAGE" value="SOIC-8" constant="no"/>
<attribute name="SUPPLIER" value="Atmel" constant="no"/>
</technology>
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
<part name="JP1" library="pinhead" deviceset="PINHD-1X4" device=""/>
<part name="JP2" library="pinhead" deviceset="PINHD-1X4" device=""/>
<part name="U1" library="Atmel-ATTINY13-20SU" deviceset="ATTINY13-20SU" device=""/>
</parts>
<sheets>
<sheet>
<plain>
</plain>
<instances>
<instance part="JP1" gate="A" x="132.08" y="48.26"/>
<instance part="JP2" gate="A" x="30.48" y="48.26" rot="MR0"/>
<instance part="U1" gate="A" x="83.82" y="53.34"/>
</instances>
<busses>
</busses>
<nets>
<net name="N$1" class="0">
<segment>
<pinref part="JP2" gate="A" pin="1"/>
<wire x1="33.02" y1="53.34" x2="38.1" y2="53.34" width="0.1524" layer="91"/>
<wire x1="38.1" y1="53.34" x2="38.1" y2="63.5" width="0.1524" layer="91"/>
<wire x1="38.1" y1="63.5" x2="106.68" y2="63.5" width="0.1524" layer="91"/>
<wire x1="106.68" y1="63.5" x2="106.68" y2="43.18" width="0.1524" layer="91"/>
<pinref part="U1" gate="A" pin="PB5"/>
<wire x1="106.68" y1="43.18" x2="101.6" y2="43.18" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$2" class="0">
<segment>
<pinref part="U1" gate="A" pin="PB3"/>
<wire x1="101.6" y1="48.26" x2="109.22" y2="48.26" width="0.1524" layer="91"/>
<wire x1="109.22" y1="48.26" x2="109.22" y2="66.04" width="0.1524" layer="91"/>
<wire x1="109.22" y1="66.04" x2="40.64" y2="66.04" width="0.1524" layer="91"/>
<wire x1="40.64" y1="66.04" x2="40.64" y2="50.8" width="0.1524" layer="91"/>
<pinref part="JP2" gate="A" pin="2"/>
<wire x1="40.64" y1="50.8" x2="33.02" y2="50.8" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$3" class="0">
<segment>
<pinref part="JP2" gate="A" pin="3"/>
<wire x1="33.02" y1="48.26" x2="43.18" y2="48.26" width="0.1524" layer="91"/>
<wire x1="43.18" y1="48.26" x2="43.18" y2="68.58" width="0.1524" layer="91"/>
<wire x1="43.18" y1="68.58" x2="111.76" y2="68.58" width="0.1524" layer="91"/>
<wire x1="111.76" y1="68.58" x2="111.76" y2="45.72" width="0.1524" layer="91"/>
<pinref part="U1" gate="A" pin="PB4"/>
<wire x1="111.76" y1="45.72" x2="101.6" y2="45.72" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$4" class="0">
<segment>
<pinref part="U1" gate="A" pin="GND"/>
<pinref part="JP2" gate="A" pin="4"/>
<wire x1="66.04" y1="45.72" x2="33.02" y2="45.72" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$5" class="0">
<segment>
<pinref part="U1" gate="A" pin="PB0"/>
<wire x1="101.6" y1="55.88" x2="114.3" y2="55.88" width="0.1524" layer="91"/>
<wire x1="114.3" y1="55.88" x2="114.3" y2="45.72" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="4"/>
<wire x1="114.3" y1="45.72" x2="129.54" y2="45.72" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$6" class="0">
<segment>
<pinref part="U1" gate="A" pin="PB1"/>
<wire x1="101.6" y1="53.34" x2="116.84" y2="53.34" width="0.1524" layer="91"/>
<wire x1="116.84" y1="53.34" x2="116.84" y2="48.26" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="3"/>
<wire x1="116.84" y1="48.26" x2="129.54" y2="48.26" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$7" class="0">
<segment>
<pinref part="U1" gate="A" pin="PB2"/>
<pinref part="JP1" gate="A" pin="2"/>
<wire x1="101.6" y1="50.8" x2="129.54" y2="50.8" width="0.1524" layer="91"/>
</segment>
</net>
<net name="N$8" class="0">
<segment>
<pinref part="U1" gate="A" pin="VCC"/>
<wire x1="66.04" y1="55.88" x2="63.5" y2="55.88" width="0.1524" layer="91"/>
<wire x1="63.5" y1="55.88" x2="63.5" y2="71.12" width="0.1524" layer="91"/>
<wire x1="63.5" y1="71.12" x2="119.38" y2="71.12" width="0.1524" layer="91"/>
<wire x1="119.38" y1="71.12" x2="119.38" y2="53.34" width="0.1524" layer="91"/>
<pinref part="JP1" gate="A" pin="1"/>
<wire x1="119.38" y1="53.34" x2="129.54" y2="53.34" width="0.1524" layer="91"/>
</segment>
</net>
</nets>
</sheet>
</sheets>
</schematic>
</drawing>
</eagle>
