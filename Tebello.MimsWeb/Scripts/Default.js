/// <reference path="jquery-3.4.1.js" />

$(document).ready(function ()
{
        try
        {
            $('[checked]').removeAttr("checked");
        }
        catch (error)
        {
            Console.log(error);
        }
})