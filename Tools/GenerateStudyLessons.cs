// Run through PowerShell Add-Type with System.Drawing. No network or external content required.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

public static class GenerateStudyLessons
{
    private static readonly Color Ink = Color.FromArgb(30,47,65);
    private static readonly Color Blue = Color.FromArgb(32,116,167);
    private static readonly Color Red = Color.FromArgb(197,72,66);
    private static readonly Color Green = Color.FromArgb(40,133,111);
    private static Bitmap Page(string subject, string title, string subtitle, Action<Graphics> draw)
    {
        var bitmap = new Bitmap(1400,900);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.Clear(Color.FromArgb(248,246,238));
            using (var brush = new SolidBrush(subject == "MATH" ? Blue : Green)) g.FillRectangle(brush,0,0,1400,17);
            Text(g, "STUDY GALLERY  /  " + subject, 66,52,22,Green);
            Text(g,title,66,104,46,Ink,true);
            Text(g,subtitle,66,184,27,Ink);
            draw(g);
            Text(g,"LOOK  /  READ  /  TRY THE QUIZ",66,840,20,Green);
        }
        return bitmap;
    }
    private static void Text(Graphics g,string text,float x,float y,float size,Color color,bool bold=false)
    {
        using(var font=new Font("Segoe UI",size,bold?FontStyle.Bold:FontStyle.Regular,GraphicsUnit.Pixel))
        using(var brush=new SolidBrush(color)) g.DrawString(text,font,brush,x,y);
    }
    private static void Card(Graphics g,int x,int y,int width,int height)
    {
        using(var brush=new SolidBrush(Color.White)) g.FillRectangle(brush,x,y,width,height);
    }
    private static void Dots(Graphics g,int count,int x,int y,Color color)
    {
        using(var brush=new SolidBrush(color))
            for(int i=0;i<count;i++) g.FillEllipse(brush,x+i*64,y,48,48);
    }
    private static void Apple(Graphics g,int x,int y)
    {
        using(var brush=new SolidBrush(Red)) { g.FillEllipse(brush,x,y+20,110,130); g.FillEllipse(brush,x+65,y+20,110,130); }
        using(var pen=new Pen(Color.SaddleBrown,12)) g.DrawLine(pen,x+88,y+35,x+100,y-5);
        using(var brush=new SolidBrush(Green)) g.FillEllipse(brush,x+100,y,54,25);
    }
    private static void Cat(Graphics g,int x,int y)
    {
        using(var brush=new SolidBrush(Color.FromArgb(223,160,68)))
        {
            g.FillEllipse(brush,x,y+20,170,140);
            g.FillPolygon(brush,new[]{new Point(x+8,y+55),new Point(x+5,y-10),new Point(x+67,y+35)});
            g.FillPolygon(brush,new[]{new Point(x+100,y+35),new Point(x+165,y-10),new Point(x+164,y+55)});
        }
        using(var brush=new SolidBrush(Ink)) { g.FillEllipse(brush,x+42,y+76,14,18); g.FillEllipse(brush,x+112,y+76,14,18); }
        using(var pen=new Pen(Ink,3)) { g.DrawLine(pen,x+85,y+105,x+85,y+122); g.DrawLine(pen,x+25,y+108,x-10,y+100); g.DrawLine(pen,x+145,y+108,x+185,y+100); }
    }
    public static void Build(string root)
    {
        string images=Path.Combine(root,"Assets/Resources/Lessons");
        string pdfs=Path.Combine(root,"Assets/StreamingAssets/Lessons");
        Directory.CreateDirectory(images); Directory.CreateDirectory(pdfs);
        using(var image=Page("MATH","Count and discover shapes","Look carefully. These ideas appear in the Math quiz.",g=>
        {
            Card(g,66,255,600,520); Card(g,696,255,638,520);
            Text(g,"COUNTING PAIRS",100,285,29,Blue,true);
            Apple(g,115,375); Apple(g,390,375); Text(g,"+",315,411,54,Ink);
            Text(g,"1 apple + 1 apple = 2 apples",100,590,32,Ink,true);
            Text(g,"1 + 1 = 2",210,666,44,Blue,true);
            Text(g,"MEET THE TRIANGLE",732,285,29,Green,true);
            using(var pen=new Pen(Green,10)) g.DrawPolygon(pen,new[]{new Point(1010,365),new Point(855,570),new Point(1170,570)});
            Text(g,"1",902,416,32,Ink); Text(g,"2",1110,416,32,Ink); Text(g,"3",1000,583,32,Ink);
            Text(g,"A triangle has 3 sides.",775,666,36,Green,true);
        })) image.Save(Path.Combine(images,"math-image.png"),ImageFormat.Png);
        using(var image=Page("ENGLISH","Meet A, B and C","Follow the letters from left to right: A, then B, then C.",g=>
        {
            for(int i=0;i<3;i++) Card(g,66+i*430,260,408,505);
            Text(g,"A",213,286,94,Red,true); Text(g,"B",642,286,94,Blue,true); Text(g,"C",1070,286,94,Green,true);
            Apple(g,180,434);
            using(var b=new SolidBrush(Blue)) g.FillEllipse(b,610,425,165,165);
            using(var p=new Pen(Color.White,6)) { g.DrawArc(p,620,430,140,154,100,165); g.DrawLine(p,616,510,765,510); }
            Cat(g,1030,433);
            Text(g,"A is for Apple",133,650,34,Ink,true); Text(g,"B is for Ball",582,650,34,Ink,true); Text(g,"C is for Cat",1012,650,34,Ink,true);
        })) image.Save(Path.Combine(images,"english-image.png"),ImageFormat.Png);
        var math=new List<Bitmap>();
        var english=new List<Bitmap>();
        try
        {
            math.Add(Page("MATH","Adding groups","MATH READING PACK  /  PAGE 1 OF 2",g=>
            {
                Card(g,66,255,1268,240); Card(g,66,520,1268,260);
                Text(g,"3 + 2 = 5",104,284,48,Blue,true); Dots(g,3,480,300,Blue); Text(g,"+",695,291,45,Ink); Dots(g,2,770,300,Green);
                Text(g,"Count 3 blue dots and 2 green dots. There are 5 altogether.",104,407,34,Ink);
                Text(g,"2 + 4 = 6",104,550,48,Blue,true); Dots(g,2,480,565,Blue); Text(g,"+",634,556,45,Ink); Dots(g,4,710,565,Green);
                Text(g,"Joining 2 dots and 4 dots makes a group of 6.",104,680,34,Ink);
            }));
            math.Add(Page("MATH","Taking away","MATH READING PACK  /  PAGE 2 OF 2",g=>
            {
                Card(g,66,255,1268,525);
                Text(g,"Start with 5 dots. Take away 2.",110,294,42,Ink,true);
                Dots(g,5,350,405,Blue);
                using(var p=new Pen(Red,7)) for(int i=3;i<5;i++) { g.DrawLine(p,350+i*64,405,398+i*64,453); g.DrawLine(p,350+i*64,453,398+i*64,405); }
                Text(g,"5 - 2 = 3",430,517,65,Blue,true);
                Text(g,"3 dots are left. Subtraction means taking away.",110,650,38,Ink);
            }));
            english.Add(Page("ENGLISH","Opposites: big and small","ENGLISH READING PACK  /  PAGE 1 OF 2",g=>
            {
                Card(g,66,255,1268,525);
                using(var b=new SolidBrush(Blue)) { g.FillEllipse(b,235,335,235,235); g.FillEllipse(b,885,440,100,100); }
                Text(g,"BIG",305,595,45,Blue,true); Text(g,"SMALL",850,595,45,Blue,true);
                Text(g,"The opposite of BIG is SMALL.",110,695,42,Ink,true);
            }));
            english.Add(Page("ENGLISH","Spell a word. Name a colour.","ENGLISH READING PACK  /  PAGE 2 OF 2",g=>
            {
                Card(g,66,255,605,525); Card(g,701,255,633,525);
                Cat(g,255,330); Text(g,"C  A  T",230,515,62,Green,true);
                Text(g,"C _ T needs the letter A.",100,640,36,Ink);
                using(var b=new SolidBrush(Blue)) g.FillRectangle(b,865,345,280,170);
                Text(g,"BLUE",920,553,48,Blue,true);
                Text(g,"Blue is a colour word.",775,665,36,Ink);
            }));
            SavePack(images,pdfs,"math",math); SavePack(images,pdfs,"english",english);
        }
        finally { foreach(var page in math) page.Dispose(); foreach(var page in english) page.Dispose(); }
    }
    private static void SavePack(string images,string pdfs,string name,List<Bitmap> pages)
    {
        for(int i=0;i<pages.Count;i++) pages[i].Save(Path.Combine(images,name+"-page-"+(i+1)+".png"),ImageFormat.Png);
        WritePdf(Path.Combine(pdfs,name+".pdf"),pages);
    }
    private static void WritePdf(string path,List<Bitmap> pages)
    {
        using(var stream=new FileStream(path,FileMode.Create,FileAccess.Write))
        {
            var offsets=new List<long>(); offsets.Add(0);
            Action<string> write=s=>{byte[] bytes=Encoding.ASCII.GetBytes(s);stream.Write(bytes,0,bytes.Length);};
            Action<int,string> obj=(id,body)=>{offsets.Add(stream.Position);write(id+" 0 obj\n"+body+"\nendobj\n");};
            write("%PDF-1.4\n");
            obj(1,"<< /Type /Catalog /Pages 2 0 R >>");
            var kids=new StringBuilder(); for(int i=0;i<pages.Count;i++) kids.Append((3+i*3)+" 0 R ");
            obj(2,"<< /Type /Pages /Count "+pages.Count+" /Kids ["+kids+"] >>");
            for(int i=0;i<pages.Count;i++)
            {
                int id=3+i*3;
                obj(id,"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 840 540] /Resources << /XObject << /Im0 "+(id+2)+" 0 R >> >> /Contents "+(id+1)+" 0 R >>");
                string commands="q 840 0 0 540 0 0 cm /Im0 Do Q\n";
                obj(id+1,"<< /Length "+commands.Length+" >>\nstream\n"+commands+"endstream");
                using(var jpeg=new MemoryStream())
                {
                    pages[i].Save(jpeg,ImageFormat.Jpeg); byte[] bytes=jpeg.ToArray();
                    offsets.Add(stream.Position);write((id+2)+" 0 obj\n<< /Type /XObject /Subtype /Image /Width 1400 /Height 900 /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length "+bytes.Length+" >>\nstream\n");
                    stream.Write(bytes,0,bytes.Length);write("\nendstream\nendobj\n");
                }
            }
            long xref=stream.Position; write("xref\n0 "+offsets.Count+"\n0000000000 65535 f \n");
            for(int i=1;i<offsets.Count;i++) write(offsets[i].ToString("D10")+" 00000 n \n");
            write("trailer\n<< /Size "+offsets.Count+" /Root 1 0 R >>\nstartxref\n"+xref+"\n%%EOF\n");
        }
    }
}
