﻿using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using Century21.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Century21.Models
{
    public class FilterProperties
    {
        public string city { get; set; }
        public string price { get; set; }
        public string beds { get; set; }
        public string beds1 { get; set; }
        public string Listtype { get; set; }
        public string price1 { get; set; }
        public List<SelectListItem> getprices { get; set; }
        public List<SelectListItem> getBeds { get; set; }
        public List<SelectListItem> getRooms { get; set; }

        public List<storedResult> storedProperties { get; set; }
        public List<SelectListItem> getprices1 { get; set; }
        public storedResult storedproperties1 { get; set; }

        public List<SelectListItem> GetPriceList()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="0-100000",Text="$0-$100000"},
                 new SelectListItem{ Value="100000-300000",Text="$100000-$300000"},
                 new SelectListItem{ Value="300000-500000",Text="$300000-$500000"},
                 new SelectListItem{ Value="500000-1000000",Text="$500000-$1000000"},
                 new SelectListItem{ Value="1000000-5000000",Text="$1000000-$5000000"},
                 new SelectListItem{ Value="5000000-10000000",Text="$5000000-$10000000"}
             };

            myList = data.ToList();
            return myList;
        }
        public List<SelectListItem> GetPriceList1()
        {
            List<SelectListItem> myList = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="0-100000",Text="$0-$100000"},
                 new SelectListItem{ Value="100000-300000",Text="$100000-$300000"},
                 new SelectListItem{ Value="300000-500000",Text="$300000-$500000"},
                 new SelectListItem{ Value="500000-1000000",Text="$500000-$1000000"},
                 new SelectListItem{ Value="1000000-5000000",Text="$1000000-$5000000"},
                 new SelectListItem{ Value="5000000-10000000",Text="$5000000-$10000000"}
             };




            myList = data.ToList();
            return myList;
        }

        public List<SelectListItem> GetBedsList()
        {
            List<SelectListItem> BedList = new List<SelectListItem>();
            var data = new[]{                    
                 new SelectListItem{ Value="0",Text="0+"},
                 new SelectListItem{ Value="1",Text="1+"},
                 new SelectListItem{ Value="2",Text="2+"},
                 new SelectListItem{ Value="3",Text="3+"},
                 new SelectListItem{ Value="4",Text="4+"},
                 new SelectListItem{ Value="5",Text="5+"},
                 new SelectListItem{ Value="6",Text="6+"},
              
             };

            BedList = data.ToList();
            return BedList;
        }

        public List<SelectListItem> GetRoomsList()
        {

            List<SelectListItem> RoomList = new List<SelectListItem>();
            var data = new[]{                    
                 new SelectListItem{ Value="Residential",Text="Residential"},
                   new SelectListItem{ Value="Rental",Text="Rental"},
                 new SelectListItem{ Value="Condo/Co-op",Text="Condo/Co-op"},
                 new SelectListItem{ Value="Commercial",Text="Commercial"},
                 new SelectListItem{ Value="Sold",Text="Sold"},
             };

            RoomList = data.ToList();
            return RoomList;

        }

    }

    public class storedResult
    {
        public string nextmlNumber { get; set; }
        public string prevmlNumber { get; set; }
        public string nextCity { get; set; }
        public string prevCity { get; set; }

        public bool IsNextVisible { get; set; }
        public bool IsPrevVisible { get; set; }

        public int currentIndex { get; set; }

        public string mls_account { get; set; }
        public string url { get; set; }
        public Nullable<int> mls { get; set; }
        public Nullable<System.DateTime> date_added { get; set; }
        public string retsysid { get; set; }
        public string ml_number { get; set; }
        public Nullable<System.DateTime> list_date { get; set; }
        public Nullable<float> list_price { get; set; }
        public Nullable<int> days_on_market { get; set; }
        public Nullable<System.DateTime> contract_date { get; set; }
        public Nullable<float> contract_price { get; set; }
        public Nullable<int> contract_dom { get; set; }
        public Nullable<System.DateTime> sell_date { get; set; }
        public Nullable<float> sell_price { get; set; }
        public string exp_date { get; set; }
        public string list_status { get; set; }
        public string list_type { get; set; }
        public string prop_type { get; set; }
        public string address { get; set; }
        public string street_number { get; set; }
        public string unit_number { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string county { get; set; }
        public string access_24hr { get; set; }
        public string acres { get; set; }
        public string adult_comm { get; set; }
        public string age { get; set; }
        public string ac { get; set; }
        public string adtext { get; set; }
        public string add_fee_freq { get; set; }
        public string add_fee_des { get; set; }
        public string add_fees { get; set; }
        public string add_fees_amt { get; set; }
        public string amenities { get; set; }
        public string amps { get; set; }
        public string appearance { get; set; }
        public string appliances { get; set; }
        public string app_req { get; set; }
        public string apt2_leased { get; set; }
        public string apt_loc { get; set; }
        public string area { get; set; }
        public string area_name { get; set; }
        public string association { get; set; }
        public string attic { get; set; }
        public string aux_rooms { get; set; }
        public string basement { get; set; }
        public string basement_finished { get; set; }
        public string basement_descr { get; set; }
        public string baths_disp { get; set; }
        public string baths { get; set; }
        public string baths_full { get; set; }
        public string baths_half { get; set; }
        public string baths_tq { get; set; }
        public string baths_1q { get; set; }
        public string board_health_ap { get; set; }
        public string beachrights { get; set; }
        public string beds { get; set; }
        public string block { get; set; }
        public string business_age { get; set; }
        public string building_approved { get; set; }
        public string building_sqft { get; set; }
        public string brs { get; set; }
        public string brslisting { get; set; }
        public string bulkhead { get; set; }
        public string bus_name { get; set; }
        public string bus_type { get; set; }
        public string cable { get; set; }
        public string caczones { get; set; }
        public string carpet { get; set; }
        public string ceiling_ht { get; set; }
        public string cofo { get; set; }
        public string comm_charges { get; set; }
        public string construct_type { get; set; }
        public string coop_shares { get; set; }
        public string county_use { get; set; }
        public string cross_st { get; set; }
        public string culdesac { get; set; }
        public string deck { get; set; }
        public string deduct_per { get; set; }
        public string deposit { get; set; }
        public string desc1 { get; set; }
        public string desc2 { get; set; }
        public string desc3 { get; set; }
        public string desc4 { get; set; }
        public string desc5 { get; set; }
        public string desc6 { get; set; }
        public string desc7 { get; set; }
        public string desc8 { get; set; }
        public string detattached { get; set; }
        public string development { get; set; }
        public string dining_room { get; set; }
        public string directions { get; set; }
        public string dishwasher { get; set; }
        public string dock { get; set; }
        public string driveway { get; set; }
        public string dryer { get; set; }
        public string easements { get; set; }
        public string eik { get; set; }
        public string electric { get; set; }
        public string electric_avail { get; set; }
        public string electric_meter { get; set; }
        public string elevator { get; set; }
        public string end_unit { get; set; }
        public string energyeff_1 { get; set; }
        public string energyeff_2 { get; set; }
        public string energyeff_3 { get; set; }
        public string energyeff_4 { get; set; }
        public string energyeff_5 { get; set; }
        public string energyeff_6 { get; set; }
        public string energyeff_7 { get; set; }
        public string energyeff_8 { get; set; }
        public string energyeff_9 { get; set; }
        public string energyeff_10 { get; set; }
        public string energyeff_11 { get; set; }
        public string energyeff_12 { get; set; }
        public string energyeff_13 { get; set; }
        public string energyeff_14 { get; set; }
        public string energyeff_15 { get; set; }
        public string energyeff_16 { get; set; }
        public string energyeff_17 { get; set; }
        public string energyeff_18 { get; set; }
        public string energyeff_19 { get; set; }
        public string energyeff_20 { get; set; }
        public string excluded { get; set; }
        public string expenses { get; set; }
        public string expenses_inc { get; set; }
        public string exterior { get; set; }
        public string extras { get; set; }
        public string extra_land { get; set; }
        public string families { get; set; }
        public string fence { get; set; }
        public string fireplaces { get; set; }
        public string fix_eqp { get; set; }
        public string flip_tax { get; set; }
        public string flip_tax_amt { get; set; }
        public string flr_ld { get; set; }
        public string fl_number { get; set; }
        public string floors { get; set; }
        public string foreclosure { get; set; }
        public string foundation { get; set; }
        public string ftr_garage { get; set; }
        public string front_ft { get; set; }
        public string fuel { get; set; }
        public string furnished { get; set; }
        public string garage { get; set; }
        public string garage_type { get; set; }
        public string garage_capacity { get; set; }
        public string garbage { get; set; }
        public string gas_avail { get; set; }
        public string gas_meter { get; set; }
        public string gated { get; set; }
        public string gov_pending { get; set; }
        public string green_cert { get; set; }
        public string green_cert_type { get; set; }
        public string green_cert_year { get; set; }
        public string greenfeatures { get; set; }
        public string greenfeatures_desc { get; set; }
        public string gross_inc { get; set; }
        public string ground_care { get; set; }
        public string handicap { get; set; }
        public string handicap_desc { get; set; }
        public string heat { get; set; }
        public string heat_sys { get; set; }
        public string heat_units { get; set; }
        public string hoa { get; set; }
        public string hoa_fee_per { get; set; }
        public string hoa_fees_inc { get; set; }
        public string horseprop { get; set; }
        public string hot_wt { get; set; }
        public string house_color { get; set; }
        public string house_dimen { get; set; }
        public string house_keep { get; set; }
        public string improvements { get; set; }
        public string inc1_dol { get; set; }
        public string inc2_dol { get; set; }
        public string inc3_dol { get; set; }
        public string inc4_dol { get; set; }
        public string inc5_dol { get; set; }
        public string inc6_dol { get; set; }
        public string inc7_dol { get; set; }
        public string inc8_dol { get; set; }
        public string included { get; set; }
        public string insurance { get; set; }
        public string inventory { get; set; }
        public string interior { get; set; }
        public string internet { get; set; }
        public string kitchen { get; set; }
        public string land_use { get; set; }
        public Nullable<decimal> latitude { get; set; }
        public string leasetype { get; set; }
        public string lease_yrs { get; set; }
        public string lease_hel_imp { get; set; }
        public string lease_purch { get; set; }
        public string level_roll { get; set; }
        public string levels { get; set; }
        public string level1 { get; set; }
        public string level2 { get; set; }
        public string level3 { get; set; }
        public string levelb { get; set; }
        public string living_room { get; set; }
        public string load_dock { get; set; }
        public Nullable<decimal> longitude { get; set; }
        public string lot { get; set; }
        public string lot_size { get; set; }
        public string lot_sqft { get; set; }
        public string lot_frontage { get; set; }
        public string maint { get; set; }
        public string maint_deduct { get; set; }
        public string maint_rep { get; set; }
        public string max_finance { get; set; }
        public string main_bath { get; set; }
        public string main_unit_leased { get; set; }
        public string master_br { get; set; }
        public string mgmt { get; set; }
        public string minage { get; set; }
        public string min_plot { get; set; }
        public string model { get; set; }
        public string neighborhood { get; set; }
        public string net_inc { get; set; }
        public string new_constr { get; set; }
        public string num_1bd { get; set; }
        public string num2_bd { get; set; }
        public string num3_bd { get; set; }
        public string num4_bd { get; set; }
        public string num_buildings { get; set; }
        public string num_com_units { get; set; }
        public string num_fl_units { get; set; }
        public string num_kit { get; set; }
        public string num_man_units { get; set; }
        public string num_med_units { get; set; }
        public string num_nco_units { get; set; }
        public string num_res_units { get; set; }
        public string occupy { get; set; }
        public string occ_date { get; set; }
        public string oth_approval { get; set; }
        public string office { get; set; }
        public string office_perc { get; set; }
        public string owner_prop { get; set; }
        public string owner_add { get; set; }
        public string owner_phone { get; set; }
        public string owner_email { get; set; }
        public string owner_cell { get; set; }
        public string own_w_sub { get; set; }
        public string opt_buy { get; set; }
        public string other { get; set; }
        public string out_of_area { get; set; }
        public string owner_fin { get; set; }
        public string owner_occ { get; set; }
        public string owner_pays { get; set; }
        public string parking { get; set; }
        public string parking_fees { get; set; }
        public string parking_spaces { get; set; }
        public string patio { get; set; }
        public string patio_terr { get; set; }
        public string payroll { get; set; }
        public string paved { get; set; }
        public string permitted_use { get; set; }
        public string pets { get; set; }
        public string pic { get; set; }
        public string plans_approve { get; set; }
        public string pool { get; set; }
        public string poll_care { get; set; }
        public string pool_desc { get; set; }
        public string possession { get; set; }
        public string poss_subdiv { get; set; }
        public string post_office { get; set; }
        public string porch { get; set; }
        public string pp_sf { get; set; }
        public string prop_disc { get; set; }
        public string prop_subtype1 { get; set; }
        public string prop_subtype2 { get; set; }
        public string prop_subtype3 { get; set; }
        public string public_record { get; set; }
        public string public_private { get; set; }
        public string priv_entrance { get; set; }
        public string raw_land { get; set; }
        public string refrigerator { get; set; }
        public string rent_control { get; set; }
        public string rent_stable { get; set; }
        public string rent_inc { get; set; }
        public string rent1 { get; set; }
        public string rent2 { get; set; }
        public string rent3 { get; set; }
        public string rent4 { get; set; }
        public string rent5 { get; set; }
        public string rent6 { get; set; }
        public string rent_allowed { get; set; }
        public string rent_sqft { get; set; }
        public string rent_program { get; set; }
        public string rent_program_type { get; set; }
        public string REO { get; set; }
        public string reserve { get; set; }
        public string road_surface { get; set; }
        public string roof { get; set; }
        public string rooms { get; set; }
        public string s_r { get; set; }
        public string sale_type { get; set; }
        public string sd_name { get; set; }
        public string sd_name2 { get; set; }
        public string sd_name3 { get; set; }
        public string sd_name4 { get; set; }
        public string sd_number { get; set; }
        public string sd_type { get; set; }
        public string sd_type2 { get; set; }
        public string sd_type3 { get; set; }
        public string sd_type4 { get; set; }
        public string shares { get; set; }
        public string seats { get; set; }
        public string securitydep { get; set; }
        public string sep_thermostat { get; set; }
        public string seasonal { get; set; }
        public Nullable<float> AugLaborDayamt { get; set; }
        public Nullable<float> Julyamt { get; set; }
        public Nullable<float> Juneamt { get; set; }
        public Nullable<float> memdaylabordayamt { get; set; }
        public Nullable<float> offseasonamt { get; set; }
        public string offseason_desc { get; set; }
        public Nullable<float> weeklyamt { get; set; }
        public Nullable<float> yr_roundamt { get; set; }
        public string whole_house_amt { get; set; }
        public string section_area { get; set; }
        public string seller_concession { get; set; }
        public string septic_app { get; set; }
        public string sewer { get; set; }
        public string short_sale { get; set; }
        public string show_address { get; set; }
        public string showing_instr { get; set; }
        public string siding { get; set; }
        public string skylight { get; set; }
        public string smoking { get; set; }
        public string sprinklers { get; set; }
        public string sqft { get; set; }
        public string stories { get; set; }
        public string stove { get; set; }
        public string style { get; set; }
        public string survey { get; set; }
        public string taxid { get; set; }
        public string tax_assessed { get; set; }
        public string tax_source { get; set; }
        public string tax_yr { get; set; }
        public Nullable<float> taxes { get; set; }
        public string term { get; set; }
        public string topography { get; set; }
        public string total_taxes { get; set; }
        public string total_exp { get; set; }
        public string total_income { get; set; }
        public string tax_abated { get; set; }
        public string tennis { get; set; }
        public string tennis_desc { get; set; }
        public string tenant_pays { get; set; }
        public string tenant_req { get; set; }
        public string terms { get; set; }
        public string total_units { get; set; }
        public string type_own { get; set; }
        public string triple_net { get; set; }
        public string unit2_appliances { get; set; }
        public string unit2_baths_full { get; set; }
        public string unit2_baths_half { get; set; }
        public string unit2_baths_tq { get; set; }
        public string unit2_beds { get; set; }
        public string unit2_desc { get; set; }
        public string unit2_rooms { get; set; }
        public string use_restrict { get; set; }
        public string vac_perc { get; set; }
        public string variance_needed { get; set; }
        public string videourl { get; set; }
        public string village { get; set; }
        public string village_tax { get; set; }
        public string virtual_tour { get; set; }
        public string virtual_tour2 { get; set; }
        public string virtual_tour3 { get; set; }
        public string virtual_tour4 { get; set; }
        public string VOW_AVM { get; set; }
        public string VOW_3rd { get; set; }
        public string walls { get; set; }
        public string washer { get; set; }
        public string water { get; set; }
        public string water_heater { get; set; }
        public string waterfront { get; set; }
        public string waterfront_desc { get; set; }
        public string water_frontage { get; set; }
        public string waterview { get; set; }
        public string weekend_service { get; set; }
        public string well { get; set; }
        public string woodfloors { get; set; }
        public string yard { get; set; }
        public string yr_built { get; set; }
        public string yr_built_descr { get; set; }
        public string zone_number { get; set; }
        public string zoning { get; set; }
        public string list_agent_id { get; set; }
        public string list_agent { get; set; }
        public string co_list_agent_id { get; set; }
        public string co_list_agent { get; set; }
        public string sell_agent_id { get; set; }
        public string sell_agent { get; set; }
        public string co_sell_agent_id { get; set; }
        public string co_sell_agent { get; set; }
        public string realtor { get; set; }
        public string realtor_code { get; set; }
        public string list_branch { get; set; }
        public string sell_realtor { get; set; }
        public string sell_realtor_code { get; set; }
        public string sell_branch { get; set; }
        public string openhouse_date_start { get; set; }
        public string openhouse_time_start { get; set; }
        public string openhouse_date_end { get; set; }
        public string openhouse_time_end { get; set; }
        public Nullable<int> @override { get; set; }
        public int FMPID { get; set; }
        public string st_dir { get; set; }
        public string st_sfx { get; set; }
        public string basement_slfr { get; set; }
        public string st_dir_sfx { get; set; }
        public string openhouse_date_start2 { get; set; }
        public string openhouse_time_start2 { get; set; }
        public string openhouse_date_end2 { get; set; }
        public string openhouse_time_end2 { get; set; }
        public string openhouse_date_start3 { get; set; }
        public string openhouse_time_start3 { get; set; }
        public string openhouse_date_end3 { get; set; }
        public string openhouse_time_end3 { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string FIRST_NAME { get; set; }



    }
}
