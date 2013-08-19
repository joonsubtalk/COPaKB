<%@ Page Title="Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Maintenance" Language="C#" AutoEventWireup="true" CodeFile="maintenance.aspx.cs" Inherits="maintenance" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <title>Cardiac Organellar Protein Atlas Knowledgebase (COPaKB) —— Maintenance</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="Joon-Sub Chung">
    <link rel="stylesheet" type="text/css" media="all" href="./assets/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" media="all" href="./assets/css/bootstrap-responsive.min.css">
    <style type="text/css">

        body {
            padding-bottom: 20px;
            font-size: 12px;
        }
        .logoplace {
            padding-bottom: 50px;
        }
        .footer {
            padding-top: 200px;
            color: #888;
        }

        .footer > hr {
            margin: 5px 0;
            border: 0;
            border-top: 1px solid #eeeeee;
            border-bottom: 1px solid #ffffff;
        }

        /* Custom container */
        .container {
            margin: 0 auto;
            max-width: 1000px;
        }

        .container > hr {
            margin: 0 0 60px 0;
            border: none;
        }

        .bar-left {
            border-left: 1px solid #aaa;
            padding-left: 10px;
        }

        h2 {
            font-size: 20px;
        }

        .top-bar {
            background-color: #0085ca;
            color: #fff;
            height: 20px;
            padding: 5px;
            margin-bottom: 10px;
        }
    </style>
    
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>

    <script type="text/javascript">

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-23316083-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

</script>
</head>
<body>
<div class="container">
    <div class="row-fluid">
        <div class="row-fluid">
            <div class="span12">
                <div class="logoplace">
                    <img src="./assets/img/copamed.jpg" alt="" />
                </div>
                <div class="alert alert-danger">
                    <h2>This website is temporarily down for maintenance.</h2>
                    <p>We are performing scheduled maintenance. We should be back online shortly. Thank you for your patience.</p>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="footer">
            <div class="row-fluid">
                <div class="span12">
                    <div class="row-fluid">
                        <div class="span6">
                            &copy; COPaKB 2013 | Cardiovascular Research Laboratory<br />
                            Suite 1-609 MRL Building 675 Charles E. Young Dr. South Los Angeles, CA 90095-1760
                        </div>
                        <div class="offset3 span4">
                            <a href="COPaWikiDefault.aspx?PageName=COPaKB+Help+Desk">Help Desk</a> | <a href="COPaWikiDefault.aspx?PageName=FUNCTIONS%20AND%20UTILTIES%20OF%20COPaKB">Tutorials</a> | <a href="COPaWikiDefault.aspx">COPaKB Wiki</a> | <a href="#">Private Policy</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row-fluid">
                <hr class="foot">
                <p>
                    COPa Knowledgebase is supported by a Proteomics Center Award from NHLBI/NIH<br />
                    268201000035C Proteome Biology of Cardiovascular Disease
                </p>
            </div>
        </div>
    </div>
</div> <!-- /container -->
</body>
</html>