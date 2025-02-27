using System;
using System.Collections.Generic;
using System.IO;
using com.knapp.CodingContest.warehouse;
using com.knapp.CodingContest.core;
using com.knapp.CodingContest.solution;
using System.Diagnostics;
using System.Linq;
using com.knapp.CodingContest.data;

namespace com.knapp.CodingContest
{
    static class Program
    {

        /// <summary>
        /// you may change any code you like
        ///      => but changing the output may lead to invalid results!
        /// </summary>
        static void Main( )
        {
            Console.Out.WriteLine("KNAPP Coding Contest 2021: Starting...");
            InputDataInternal iinput;
            InputStat istat;

            try
            {
                Console.Out.WriteLine("#... LOADING Input ...");
                iinput = new InputDataInternal();
                iinput.Load( );

                Console.Out.WriteLine(iinput.WarehouseCharacteristics);
                istat = new InputStat(iinput);

                Console.Out.WriteLine();

                Console.Out.WriteLine( "#... DATA LOADED" );
            }
            catch ( Exception e )
            {
                ShowException( e, "Exception in startup code" );
                Console.Out.WriteLine( "Press <enter>" );
                Console.In.ReadLine( );
                throw;
            }

            Console.Out.WriteLine( "### Your output starts here" );

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            WarehouseInternal warehouse = new WarehouseInternal(iinput);

            Solution solution = new Solution(warehouse, iinput);
            try
            {

                solution.Run();
                stopwatch.Stop();
            }
            catch (Exception e)
            {
                ShowException(e, "Exception in your code");
                Console.Out.WriteLine("Press <enter>");
                Console.In.ReadLine();
                throw;
            }

            WriteProperties(solution, Settings.outPropertyFilename);

            Console.Out.WriteLine($"### DONE in {stopwatch.ElapsedMilliseconds}ms");
            Console.Out.WriteLine($"------------------------------------------------");
            Console.Out.WriteLine($"   Results/ Cost for your solution");
            Console.Out.WriteLine($"   {solution.ParticipantName} / {solution.Institute}");

            WriteResults( istat, warehouse );

            try
            {
                PrepareUpload.WriteResult( warehouse, warehouse.GetResult() );
                PrepareUpload.CreateZipFile();
                Console.Out.WriteLine( ">>> Created " + Settings.outZipFilename );
            }
            catch ( Exception e )
            {
                ShowException( e, "Exception in shutdown code" );
                throw;
            }

            Console.Out.WriteLine( "Press <enter>" );
            Console.In.ReadLine( );
        }


        private static void WriteResults( InputStat  istat, WarehouseInternal whi )
        {
            IWarehouseInfo info = whi.GetInfoSnapshot();
            IWarehouseCostFactors c = whi.GetCostFactors();

            int uo = info.GetUnfinishedOrderCount();
            int upe = info.GetUnfinishedProductStillAtEntryCount();
            int ups = info.GetUnfinishedProductInStorageCount();
            long dl = info.GetDistanceLevel();
            long dp = info.GetDistancePosition();
            double dd = info.GetDistanceDirect();

            double c_d = info.GetDistanceCost();
            double c_t = info.GetTotalCost();

            double c_uo = info.GetUnfinishedOrdersCost();
            double c_upe = info.GetUnfinishedProductsStillAtEntryCost();
            double c_ups = info.GetUnfinishedProductsInStorageCost();

            //

            int ps = whi.GetStorage().GetAllLocations().Select(l => l.GetCurrentProducts().Count).Sum();
            double m_o = (double)whi.Moves / istat.Orders;
            double m_p = (double)whi.Moves / istat.ProductsOrders;
            double d_o = dd / istat.Orders;
            double d_p = dd / istat.ProductsOrders;

            Console.Out.WriteLine("");
            Console.Out.WriteLine("  --------------------------------------------------------------");
            Console.Out.WriteLine("    INPUT STATISTICS                                            ");
            Console.Out.WriteLine("  ------------------------------------- : ----------------------");
            Console.Out.WriteLine("      #orders                           :  {1,8}", "", istat.Orders);
            Console.Out.WriteLine("      #products (order)                 :  {1,8}", "", istat.ProductsOrders);
            Console.Out.WriteLine("      #products (entry)                 :  {1,8}  +{2}", "", istat.ProductsInqueue, (istat.ProductsInqueue-istat.ProductsOrders) );
            Console.Out.WriteLine("      #products / order                 :  {1,10:0.0}", "", istat.ProductsPerOrder );
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  --------------------------------------------------------------");
            Console.Out.WriteLine("    RESULT STATISTICS                                           ");
            Console.Out.WriteLine("  ------------------------------------- : ----------------------");
            Console.Out.WriteLine("      # products still at entry         :  {0,8}", whi.GetRemainingProductsAtEntry().Count);
            Console.Out.WriteLine("      # products in storage             :  {0,8}", ps);
            Console.Out.WriteLine("      # moves                           :  {0,8}", whi.Moves);
            Console.Out.WriteLine("      # moves / order                   :  {0,10:0.0}", m_o);
            Console.Out.WriteLine("      # moves / product (order)         :  {0,10:0.0}", m_p);
            Console.Out.WriteLine("      distance / order                  :  {0,10:0.0}", d_o);
            Console.Out.WriteLine("      distance / product (order)        :  {0,10:0.0}", d_p);
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  =============================================================================");
            Console.Out.WriteLine("    RESULTS                                                                    ");
            Console.Out.WriteLine("  ===================================== : ============ | ======================");
            Console.Out.WriteLine("      what                              :       costs  |  (details: count,...)");
            Console.Out.WriteLine("  ------------------------------------- : ------------ | ----------------------");
            Console.Out.WriteLine("   -> distance level                    :  {0,8}    |   {1,6}", "", dl);
            Console.Out.WriteLine("   -> distance position                 :  {0,8}    |   {1,6}", "", dp);
            Console.Out.WriteLine("   -> distance direct                   :  {0,10:0.0}  |   {1,8:0.0}", c_d, dd);
            Console.Out.WriteLine("   -> unfinished orders                 :  {0,10:0.0}  |   {1,6}", c_uo, uo);
            Console.Out.WriteLine("   -> unfinished products (at entry)    :  {0,10:0.0}  |   {1,6}", c_upe, upe);
            Console.Out.WriteLine("   -> unfinished products (in storage)  :  {0,10:0.0}  |   {1,6}", c_ups, ups);
            Console.Out.WriteLine("  ------------------------------------- : ------------ | ----------------------");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("   => TOTAL COST                           {0,10:0.0}", c_t);
            Console.Out.WriteLine("                                          ============");

        }

        /// <summary>
        /// Helper function to write the properties to the file
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="outFilename"></param>
        /// <exception cref="ArgumentException">when either solution.InstituteId or solution.ParticipantName is not valid</exception>
        private static void WriteProperties(Solution solution, string outFilename)
        {
            if (File.Exists(outFilename))
            {
                File.Delete( outFilename);
            }

            using ( StreamWriter stream = new StreamWriter( outFilename, false, System.Text.Encoding.GetEncoding( "ISO-8859-1" ) ) )
            {
                stream.WriteLine( "# -*- conf-javaprop -*-" );
                stream.WriteLine( $"participant = {solution.ParticipantName}" );
                stream.WriteLine( $"institution = {solution.Institute}" );
                stream.WriteLine( "technology = c#" );
            }

        }

        /// <summary>
        /// write exception to console.error
        /// includes inner exception and data
        /// </summary>
        /// <param name="e">exception that should be shown</param>
        /// <param name="codeSegment">segment where the exception was caught</param>
        public static void ShowException( Exception e, string codeSegment )
        {
            Console.Out.WriteLine( "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" );
            Console.Out.WriteLine(  codeSegment );
            Console.Out.WriteLine( "[{0}]: {1}", e.GetType( ).Name, e.Message );

            for ( Exception inner = e.InnerException
                ; inner != null
                ; inner = inner.InnerException )
            {
                System.Console.WriteLine( ">>[{0}] {1}"
                                                , inner.GetType( ).Name
                                                , inner.Message
                                            );
            }


            if ( e.Data != null && e.Data.Count > 0 )
            {
                Console.Error.WriteLine( "------------------------------------------------" );
                Console.Error.WriteLine( "Data in exception:" );
                foreach( KeyValuePair<string, string> elem in e.Data )
                {
                    Console.Error.WriteLine( "[{0}] : '{1}'", elem.Key, elem.Value );
                }
            }
            Console.Out.WriteLine( "------------------------------------------------" );
            Console.Out.WriteLine( e.StackTrace );
            Console.Out.WriteLine( "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" );
        }

        public class InputStat
        {
            public int Orders { get; private set; }
            public int ProductsInqueue { get; private set; }
            public int ProductsOrders { get; private set; }
            public double ProductsPerOrder { get; private set; }

            public InputStat( InputDataInternal iinput )
            {
                Orders = iinput.GetAllOrders().Count;
                ProductsInqueue = iinput.GetAllProductsAtEntry().Count;
                ProductsOrders = iinput.GetAllOrders().Select(o => o.GetProducts().Count).Sum();
                ProductsPerOrder = (double)ProductsOrders / Orders;
            }
        }
    }
}
