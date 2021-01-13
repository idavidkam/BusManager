﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;
namespace Dal
{
    public class DalXML : IDal
    {
        #region singelton
        static readonly DalXML instance = new DalXML();
        static DalXML() { }// static ctor to ensure instance init is done just before first usage
        DalXML() { } // default => private
        public static DalXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files
        string busesPath = @"BusesXml.xml"; //XElement

        string runNumbersPath = @"runingNumbersXml.xml";//XMLSerializer
        string busStopsPath = @"BusStopsXml.xml"; //XMLSerializer
        string busesOnTripPath = @"BusesOnTripXml.xml"; //XMLSerializer
        string consecutiveStopsPath = @"ConsecutiveStopsXml.xml"; //XMLSerializer
        string driversPath = @"DriversXml.xml"; //XMLSerializer
        string linesPath = @"LinesXml.xml"; //XMLSerializer
        string lineTripsPath = @"LineTripsXml.xml"; //XMLSerializer
        string stopLinesPath = @"StopLinesXml.xml"; //XMLSerializer
        string userTripsPath = @"UserTripsXml.xml"; //XMLSerializer
        string usersPath = @"UsersXml.xml"; //XMLSerializer
        #endregion

        #region Bus
        public Bus GetBus(int id)
        {
            XElement busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            Bus b = (from bus in busesRootElem.Elements()
                     where int.Parse(bus.Element("Id").Value) == id &&
                     bool.Parse(bus.Element("Active").Value) == true
                     select new Bus()
                     {
                         Active = true,
                         Id = uint.Parse(bus.Element("Id").Value),
                         DateRoadAscent = DateTime.Parse(bus.Element("DateRoadAscent").Value),
                         Fuel = int.Parse(bus.Element("Fuel").Value),
                         LastCare = DateTime.Parse(bus.Element("LastCare").Value),
                         LastCareMileage = uint.Parse(bus.Element("LastCareMileage").Value),
                         Mileage = uint.Parse(bus.Element("Mileage").Value),
                         State = (States)int.Parse(bus.Element("State").Value)
                     }
                   ).FirstOrDefault();
            return b;//if b==null return null
        }

        public IEnumerable<Bus> GetBuses()
        {
            XElement busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);
            return from bus in busesRootElem.Elements()
                   where bool.Parse(bus.Element("Active").Value) == true
                   select new Bus()
                   {
                       Active = bool.Parse(bus.Element("Active").Value),
                       Id = uint.Parse(bus.Element("Id").Value),
                       DateRoadAscent = DateTime.Parse(bus.Element("DateRoadAscent").Value),
                       Mileage = uint.Parse(bus.Element("Mileage").Value),
                       Fuel = int.Parse(bus.Element("Fuel").Value),
                       LastCare = DateTime.Parse(bus.Element("LastCare").Value),
                       LastCareMileage = uint.Parse(bus.Element("LastCareMileage").Value),
                       State = (States)Enum.Parse(typeof(States),bus.Element("State").Value)
                   };
        }

        public IEnumerable<Bus> GetBusesBy(Predicate<Bus> predicate)
        {
            XElement busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);
            return from bus in busesRootElem.Elements()
                   let busDO = new Bus()
                   {
                       Active = true,
                       Id = uint.Parse(bus.Element("Id").Value),
                       DateRoadAscent = DateTime.Parse(bus.Element("DateRoadAscent").Value),
                       Fuel = int.Parse(bus.Element("Fuel").Value),
                       LastCare = DateTime.Parse(bus.Element("LastCare").Value),
                       LastCareMileage = uint.Parse(bus.Element("LastCareMileage").Value),
                       Mileage = uint.Parse(bus.Element("Mileage").Value),
                       State = (States)int.Parse(bus.Element("State").Value)
                   }
                   where busDO.Active == true && predicate(busDO)
                   select busDO;

        }

        public void AddBus(Bus bus)
        {
            XElement busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var busElem = (from b in busesRootElem.Elements()
                           where int.Parse(b.Element("Id").Value) == bus.Id
                           select b).FirstOrDefault();
            if (busElem != null && bool.Parse(busElem.Element("Active").Value) == true)
                throw new BusExceptionDO((int)bus.Id, "the bus is already exists");
            if (busElem == null)
            {
                busElem = new XElement("Bus",
                          new XElement("Active", bus.Active),
                          new XElement("Id", bus.Id),
                          new XElement("DateRoadAscent", bus.DateRoadAscent),
                          new XElement("Mileage", bus.Mileage),
                          new XElement("Fuel", bus.Fuel),
                          new XElement("LastCare", bus.LastCare),
                          new XElement("LastCareMileage", bus.LastCareMileage),
                          new XElement("State", bus.State));
            }
            else
            {
                busElem.Element("Active").Value = bus.Active.ToString();
                busElem.Element("Id").Value = bus.Id.ToString();
                busElem.Element("DateRoadAscent").Value = bus.DateRoadAscent.ToString();
                busElem.Element("Mileage").Value = bus.Mileage.ToString();
                busElem.Element("Fuel").Value = bus.Fuel.ToString();
                busElem.Element("LastCare").Value = bus.LastCare.ToString();
                busElem.Element("LastCareMileage").Value = bus.LastCareMileage.ToString();
                busElem.Element("State").Value = bus.State.ToString();
            }
            busesRootElem.Add(busElem);
            XMLTools.SaveListToXMLElement(busesRootElem, busesPath);
        }

        public void DeleteBus(int id)
        {
            XElement busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var busElem = (from b in busesRootElem.Elements()
                           where int.Parse(b.Element("Id").Value) == id &&
                           bool.Parse(b.Element("Active").Value) == true
                           select b).FirstOrDefault();
            if (busElem == null)
                throw new BusExceptionDO((int)id, "the bus is not exists");
            busElem.Element("Active").Value = false.ToString();

            XMLTools.SaveListToXMLElement(busesRootElem, busesPath);
        }

        public void UpdateBus(Bus newBus)
        {
            XElement busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var busElem = (from b in busesRootElem.Elements()
                           where int.Parse(b.Element("Id").Value) == newBus.Id &&
                           bool.Parse(b.Element("Active").Value) == true
                           select b).FirstOrDefault();
            if (busElem == null)
                throw new BusExceptionDO((int)newBus.Id, "system not found the bus");

            busElem.Element("Active").Value = newBus.Active.ToString();
            busElem.Element("Id").Value = newBus.Id.ToString();
            busElem.Element("DateRoadAscent").Value = newBus.DateRoadAscent.ToString();
            busElem.Element("Mileage").Value = newBus.Mileage.ToString();
            busElem.Element("Fuel").Value = newBus.Fuel.ToString();
            busElem.Element("LastCare").Value = newBus.LastCare.ToString();
            busElem.Element("LastCareMileage").Value = newBus.LastCareMileage.ToString();
            busElem.Element("State").Value = newBus.State.ToString();

            XMLTools.SaveListToXMLElement(busesRootElem, busesPath);
        }
        #endregion

        #region BusStop
        public BusStop GetBusStop(int code)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopsPath);
            BusStop bStop = ListBusStops.Find(BusStop => BusStop.Code == code && BusStop.Active);

            return bStop; // if bStop == null return null
        }

        public IEnumerable<BusStop> GetStops()
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopsPath);
            return from bStop in ListBusStops
                   where bStop.Active == true
                   select bStop;
        }

        public IEnumerable<BusStop> GetStopsBy(Predicate<BusStop> predicate)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopsPath);
            return from Stop in ListBusStops
                   where Stop.Active == true && predicate(Stop)
                   select Stop;
        }

        public void AddStop(BusStop stop)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopsPath);

            var i = ListBusStops.FindIndex(BusStop => BusStop.Code == stop.Code);
            if (i != -1)
            {
                if (ListBusStops[i].Active)
                    throw new DO.BusStopExceptionDO(stop.Code, "the busStop is already exists");
                if (!ListBusStops[i].Active)
                    ListBusStops[i] = stop;
            }
            if (i == -1)
                ListBusStops.Add(stop);
            XMLTools.SaveListToXMLSerializer(ListBusStops, busStopsPath);
        }

        public void UpdateBusStop(BusStop busStop)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopsPath);

            int index = ListBusStops.FindIndex(BusStop => BusStop.Active && BusStop.Code == busStop.Code);
            if (index == -1)
                throw new DO.BusStopExceptionDO(busStop.Code, "System not found the busStop");
            ListBusStops[index] = busStop;

            XMLTools.SaveListToXMLSerializer(ListBusStops, busStopsPath);
        }

        public void UpdateBusStop(int code, Action<BusStop> action)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopsPath);

            int index = ListBusStops.FindIndex(BusStop => BusStop.Active && BusStop.Code == code);
            if (index == -1)
                throw new BusStopExceptionDO(code, "system not found the busStop");
            action(ListBusStops[index]);

            XMLTools.SaveListToXMLSerializer(ListBusStops, busStopsPath);
        }

        public void DeleteBusStop(int code)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopsPath);

            int index = ListBusStops.FindIndex(BusStop => BusStop.Active && BusStop.Code == code);
            if (index == -1)
                throw new BusStopExceptionDO(code, "The busStop is not exists");
            ListBusStops[index].Active = false;

            XMLTools.SaveListToXMLSerializer(ListBusStops, busStopsPath);
        }
        #endregion

        #region Driver

        public void AddDriver(Driver driver)
        {
            List<Driver> ListDrivers = XMLTools.LoadListFromXMLSerializer<Driver>(driversPath);

            int index = ListDrivers.FindIndex(Driver => Driver.Id == driver.Id);
            if (index == -1)
                ListDrivers.Add(driver);
            if (index != -1)
            {
                if (ListDrivers[index].Active)
                    throw new DriverExceptionDO(driver.Id, "The driver is already exists");
                if (!ListDrivers[index].Active)
                    ListDrivers[index] = driver;
            }
            XMLTools.SaveListToXMLSerializer(ListDrivers, driversPath);
        }

        public IEnumerable<Driver> GetDrivers()
        {
            List<Driver> ListDrivers = XMLTools.LoadListFromXMLSerializer<Driver>(driversPath);
            return from Driver in ListDrivers
                   where Driver.Active == true
                   select Driver;
        }

        public IEnumerable<Driver> GetDriversBy(Predicate<Driver> predicate)
        {
            List<Driver> ListDrivers = XMLTools.LoadListFromXMLSerializer<Driver>(driversPath);

            return from Driver in ListDrivers
                   where predicate(Driver) && Driver.Active == true
                   select Driver;
        }

        public Driver GetDriver(int id)
        {
            List<Driver> ListDrivers = XMLTools.LoadListFromXMLSerializer<Driver>(driversPath);

            var driver = ListDrivers.Find(Driver => Driver.Active && Driver.Id == id);
            if (driver == null)
                return null;
            return driver;
        }

        public void UpdateDriver(Driver newDriver)
        {
            List<Driver> ListDrivers = XMLTools.LoadListFromXMLSerializer<Driver>(driversPath);

            int index = ListDrivers.FindIndex(Driver => Driver.Active && Driver.Id == newDriver.Id);
            if (index == -1)
                throw new DriverExceptionDO(newDriver.Id, "System not found the driver");
            ListDrivers[index] = newDriver;

            XMLTools.SaveListToXMLSerializer(ListDrivers, driversPath);
        }

        public void UpdateDriver(int id, Action<Driver> action)
        {
            List<Driver> ListDrivers = XMLTools.LoadListFromXMLSerializer<Driver>(driversPath);

            int index = ListDrivers.FindIndex(Driver => Driver.Active && Driver.Id == id);
            if (index == -1)
                throw new DriverExceptionDO(id, "System not found the driver");
            action(ListDrivers[index]);

            XMLTools.SaveListToXMLSerializer(ListDrivers, driversPath);
        }

        public void DeleteDriver(int id)
        {
            List<Driver> ListDrivers = XMLTools.LoadListFromXMLSerializer<Driver>(driversPath);

            int index = ListDrivers.FindIndex(Driver => Driver.Active && Driver.Id == id);
            if (index == -1)
                throw new DriverExceptionDO(id, "The driver is not exists");
            ListDrivers[index].Active = false;

            XMLTools.SaveListToXMLSerializer(ListDrivers, driversPath);
        }
        #endregion

        #region User
        public User GetUser(string userName)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            var user = ListUsers.Find(User => User.Active && User.UserName == userName);
            if (user == null)
                return null;
            return user;
        }

        public User GetUser(string phone, DateTime dateTime)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);

            var user = ListUsers.Find(User => User.Active && User.Phone == phone && User.Birthday == dateTime);
            if (user == null)
                return null;
            return user;
        }

        public void DeleteUser(string phone, DateTime dateTime)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);

            int index = ListUsers.FindIndex(User => User.Active && User.Phone == phone && User.Birthday == dateTime);
            if (index == -1)
            {
                throw new UserExceptionDO(phone, "The user is not exists");
            }
            ListUsers[index].Active = false;
            XMLTools.SaveListToXMLSerializer(ListUsers, usersPath);
        }

        public void AddUser(User user)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            int index = ListUsers.FindIndex(User => User.UserName == user.UserName);
            if (index == -1)
            {
                ListUsers.Add(user);
            }
            if (index != -1)
            {
                if (ListUsers[index].Active)
                    throw new UserExceptionDO(user.UserName, "The User is already exists");
                if (!ListUsers[index].Active)
                    ListUsers[index] = user;
            }
            XMLTools.SaveListToXMLSerializer(ListUsers, usersPath);
        }

        public void UpdateUser(User user)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);

            int index = ListUsers.FindIndex((User) => { return User.Active && User.UserName == user.UserName; });
            if (index == -1)
                throw new UserExceptionDO(user.UserName, "System not found the userName");
            ListUsers[index] = user;

            XMLTools.SaveListToXMLSerializer(ListUsers, usersPath);
        }

        public IEnumerable<User> GetUsers()
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);

            return from user in ListUsers
                   where user.Active == true
                   select user;
        }

        public IEnumerable<User> GetUsersBy(Predicate<User> predicate)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);

            return from user in ListUsers
                   where predicate(user) && user.Active == true
                   select user;
        }
        #endregion

        #region Line
        public int CreateLine(Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            //load runing numbers from "runNumbersPath"
            XElement runNumbersElem = XMLTools.LoadListFromXMLElement(runNumbersPath);
            int runNumber = int.Parse(runNumbersElem.Element("Counter").Element("LineCounter").Value);
            line.IdLine = runNumber++;
            runNumbersElem.Element("Counter").Element("LineCounter").Value = runNumber.ToString();
            XMLTools.SaveListToXMLElement(runNumbersElem, runNumbersPath);

            ListLines.Add(line);
            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
            return line.IdLine;
        }

        public Line GetLine(int idLine)
        {
            var lines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            var line = lines.Find((Line) => { return Line.Active && Line.IdLine == idLine; });
            return line;
        }

        public IEnumerable<Line> GetLines()
        {
            var lines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            return from l in lines
                   where l.Active = true
                   select l;
        }

        public IEnumerable<Line> GetLinesBy(Predicate<Line> predicate)
        {
            var lines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            return from l in lines
                   where l.Active = true && predicate(l)
                   select l;
        }

        public void UpdateLine(Line line)
        {
            var lines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            var i = lines.FindIndex(l => l.Active && l.IdLine == line.IdLine);
            if (i == -1)
                throw new LineExceptionDO(line.IdLine, "system not found the line");
            lines[i] = line;
            XMLTools.SaveListToXMLSerializer(lines, linesPath);
        }

        public void UpdateLine(int idLine, Action<Line> action)
        {
            var lines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            var i = lines.FindIndex(l => l.Active && l.IdLine == idLine);
            if (i == -1)
                throw new LineExceptionDO(idLine, "system not found the line");
            action(lines[i]);
            XMLTools.SaveListToXMLSerializer(lines, linesPath);
        }

        public void DeleteLine(int idLine)
        {
            var lines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            int i = lines.FindIndex(l => l.Active && l.IdLine == idLine);
            if (i == -1)
                throw new LineExceptionDO(idLine, "the line is not exists");
            lines[i].Active = false;
            XMLTools.SaveListToXMLSerializer(lines, linesPath);
        }
        #endregion

        #region StopLine
        public void AddStopLine(StopLine stopLine)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);

            var index = ListStopsLines.FindIndex((StopLine) =>
            {
                return StopLine.IdLine == stopLine.IdLine && StopLine.CodeStop == stopLine.CodeStop;
            });
            if (index == -1)
            {
                ListStopsLines.Add(stopLine);
                XMLTools.SaveListToXMLSerializer(ListStopsLines, stopLinesPath);
                return;
            }
            throw new StopLineExceptionDO(stopLine.IdLine, stopLine.CodeStop, "The stop Line is already exists");
        }

        public void AddRouteStops(IEnumerable<StopLine> stops)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);
            ListStopsLines.AddRange(stops);
            XMLTools.SaveListToXMLSerializer(ListStopsLines, stopLinesPath);
        }

        public StopLine GetStopLine(int idLine, int codeStop)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);
            var stopLine = ListStopsLines.Find(StopLine => StopLine.IdLine == idLine && StopLine.CodeStop == codeStop);
            if (stopLine == null)
                return null;
            return stopLine;
        }

        public StopLine GetStopLineByIndex(int idLine, int index)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);
            var stopLine = ListStopsLines.Find(StopLine => StopLine.IdLine == idLine && StopLine.NumStopInLine == index);
            if (stopLine == null)
                return null;
            return stopLine;
        }

        public IEnumerable<StopLine> GetStopLines()
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);
            return from StopLine in ListStopsLines
                   select StopLine;
        }

        public IEnumerable<StopLine> GetStopLinesBy(Predicate<StopLine> predicate)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);
            return from StopLine in ListStopsLines
                   where predicate(StopLine)
                   orderby StopLine.NumStopInLine
                   select StopLine;
        }

        public void UpdateStopLine(StopLine stopLine)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);

            int index = ListStopsLines.FindIndex((StopLine) =>
            { return StopLine.IdLine == stopLine.IdLine && StopLine.CodeStop == stopLine.CodeStop; });
            if (index == -1)
                throw new StopLineExceptionDO(stopLine.IdLine, stopLine.CodeStop, "System not found the stop line");
            ListStopsLines[index] = stopLine;

            XMLTools.SaveListToXMLSerializer(ListStopsLines, stopLinesPath);
        }

        public void UpdateStopLine(int idLine, int codeStop, Action<StopLine> action)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);

            int index = ListStopsLines.FindIndex((StopLine) =>
            { return StopLine.IdLine == idLine && StopLine.CodeStop == codeStop; });
            if (index == -1)
                throw new StopLineExceptionDO(idLine, codeStop, "System not found the stop line");
            action(ListStopsLines[index]);

            XMLTools.SaveListToXMLSerializer(ListStopsLines, stopLinesPath);
        }

        public void DeleteStopLine(int idLine, int codeStop)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);

            int index = ListStopsLines.FindIndex((StopLine) =>
            { return StopLine.IdLine == idLine && StopLine.CodeStop == codeStop; });
            if (index == -1)
                throw new StopLineExceptionDO(idLine, codeStop, "The stop line is not exists");
            ListStopsLines.RemoveAt(index);

            XMLTools.SaveListToXMLSerializer(ListStopsLines, stopLinesPath);
        }

        public void DeleteAllStopsInLine(int idLine)
        {
            List<StopLine> ListStopsLines = XMLTools.LoadListFromXMLSerializer<StopLine>(stopLinesPath);

            ListStopsLines.RemoveAll((StopLine) => StopLine.IdLine == idLine);

            XMLTools.SaveListToXMLSerializer(ListStopsLines, stopLinesPath);
        }
        #endregion

        #region ConsecutiveStops
        public ConsecutiveStops GetConsecutiveStops(int codeStop1, int codeStop2)
        {
            var LstConsecutiveStops = XMLTools.LoadListFromXMLSerializer<ConsecutiveStops>(consecutiveStopsPath);
            var conStops = LstConsecutiveStops.Find((ConsecutiveStops) =>
            {
                return ConsecutiveStops.CodeBusStop1 == codeStop1
                && ConsecutiveStops.CodeBusStop2 == codeStop2;
            });
            return conStops;
        }

        public void AddConsecutiveStops(ConsecutiveStops consecutiveStops)
        {
            var LstConsecutiveStops = XMLTools.LoadListFromXMLSerializer<ConsecutiveStops>(consecutiveStopsPath);
            int index = LstConsecutiveStops.FindIndex((ConsecutiveStops) =>
            {
                return ConsecutiveStops.CodeBusStop1 == consecutiveStops.CodeBusStop1
                 && ConsecutiveStops.CodeBusStop2 == consecutiveStops.CodeBusStop2;
            });
            if (index != -1)
            {
                throw new ConsecutiveStopsExceptionDO(consecutiveStops.CodeBusStop1, consecutiveStops.CodeBusStop2, "the stops is already exists");
            }
            LstConsecutiveStops.Add(consecutiveStops);
            XMLTools.SaveListToXMLSerializer(LstConsecutiveStops, consecutiveStopsPath);
        }

        public IEnumerable<ConsecutiveStops> GetLstConsecutiveStops()
        {
            var lstConStops = XMLTools.LoadListFromXMLSerializer<ConsecutiveStops>(consecutiveStopsPath);
            return from conStop in lstConStops
                   select conStop;
        }

        public IEnumerable<ConsecutiveStops> GetLstConsecutiveStopsBy(Predicate<ConsecutiveStops> predicate)
        {
            var lstConStops = XMLTools.LoadListFromXMLSerializer<ConsecutiveStops>(consecutiveStopsPath);
            return from conStop in lstConStops
                   where predicate(conStop)
                   select conStop;
        }

        public void UpdateConsecutiveStops(ConsecutiveStops consecutiveStops)
        {
            var lstConStops = XMLTools.LoadListFromXMLSerializer<ConsecutiveStops>(consecutiveStopsPath);
            int index = lstConStops.FindIndex((ConsecutiveStops) =>
            {
                return ConsecutiveStops.CodeBusStop1 == consecutiveStops.CodeBusStop1
                && ConsecutiveStops.CodeBusStop2 == consecutiveStops.CodeBusStop2;
            });
            if (index == -1)
                throw new ConsecutiveStopsExceptionDO(consecutiveStops.CodeBusStop1, consecutiveStops.CodeBusStop2, "system not found the consecutiveStops");
            lstConStops[index] = consecutiveStops;
            XMLTools.SaveListToXMLSerializer(lstConStops, consecutiveStopsPath);
        }

        public void UpdateConsecutiveStops(int codeStop1, int codeStop2, Action<ConsecutiveStops> action)
        {
            var lstConStops = XMLTools.LoadListFromXMLSerializer<ConsecutiveStops>(consecutiveStopsPath);
            int index = lstConStops.FindIndex((ConsecutiveStops) =>
            {
                return ConsecutiveStops.CodeBusStop1 == codeStop1
                && ConsecutiveStops.CodeBusStop2 == codeStop2;
            });
            if (index == -1)
                throw new ConsecutiveStopsExceptionDO(codeStop1, codeStop2, "System not found the consecutiveStops");
            action(lstConStops[index]);
            XMLTools.SaveListToXMLSerializer(lstConStops, consecutiveStopsPath);
        }

        #endregion

        #region LineTrip

        public int CreateLineTrip(LineTrip lineTrip)
        {
            var lineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripsPath);
            XElement runNumbersElem = XMLTools.LoadListFromXMLElement(runNumbersPath);
            int runNumber = int.Parse(runNumbersElem.Element("Counter").Element("LineTripCounter").Value);
            lineTrip.Id = runNumber++;
            runNumbersElem.Element("Counter").Element("LineTripCounter").Value = runNumber.ToString();
            XMLTools.SaveListToXMLElement(runNumbersElem, runNumbersPath);
            lineTrips.Add(lineTrip);
            XMLTools.SaveListToXMLSerializer(lineTrips, lineTripsPath);
            return lineTrip.Id;
        }

        public LineTrip GetLineTrip(int id)
        {
            var lineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripsPath);
            var lineTrip = lineTrips.Find(LineTrip=> LineTrip.Active && LineTrip.Id == id);
            if (lineTrip == null)
                return null;
            return lineTrip;
        }

        public IEnumerable<LineTrip> GetLineTrips()
        {
            var lineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripsPath);
            return from LineTrip in lineTrips
                   where LineTrip.Active == true
                   select LineTrip;
        }

        public IEnumerable<LineTrip> GetLineTripsBy(Predicate<LineTrip> predicate)
        {
            var lineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripsPath);
            return from LineTrip in lineTrips
                   where predicate(LineTrip)&&LineTrip.Active == true
                   select LineTrip;
        }

        public void UpdateLineTrip(LineTrip lineTrip)
        {
            var lineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripsPath);
            int index = lineTrips.FindIndex(LineTrip => LineTrip.Active && LineTrip.Id == lineTrip.Id);
            if (index == -1)
                throw new LineTripExceptionDO(lineTrip.Id, "System not found the line trip");
            lineTrips[index] = lineTrip;
            XMLTools.SaveListToXMLSerializer(lineTrips, lineTripsPath);
        }

        public void UpdateLineTrip(int id, Action<LineTrip> action)
        {
            var lineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripsPath);
            int index = lineTrips.FindIndex(LineTrip => LineTrip.Active && LineTrip.Id == id);
            if (index == -1)
                throw new LineTripExceptionDO(id, "System not found the line trip");
            action( lineTrips[index]);
            XMLTools.SaveListToXMLSerializer(lineTrips, lineTripsPath);
        }

        public void DeleteLineTrip(int id)
        {
            var lineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripsPath);
            int index = lineTrips.FindIndex(LineTrip => LineTrip.Active && LineTrip.Id == id);
            if (index == -1)
                throw new LineTripExceptionDO(id, "the line trip is not exists");
            lineTrips[index].Active=false;
            XMLTools.SaveListToXMLSerializer(lineTrips, lineTripsPath);
        }
        #endregion
    }
}
