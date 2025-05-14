using Models.Dtos;
using Models;
using Interfaces;
namespace Service;

public class BookingService:IBookingService
{
  private IAdminDbRepo _adminDbRepo;
  private IBookingDbRepo  _BookingDbRepo;
  public BookingService(IAdminDbRepo adminDbRepo,IBookingDbRepo BookingDbRepo)
  {
    _adminDbRepo=adminDbRepo;
    _BookingDbRepo=BookingDbRepo;
  }

  public async Task<ResponseModel> SelectAllSeatsForJourney(int journeyId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<SeatModel>>(await _BookingDbRepo.SelectAllSeatsForJourney(journeyId));
  }

  public async Task<ResponseModel> SelectBookedSeatsForJourney(int journeyId,int apartmentId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<SeatModel>>(await _BookingDbRepo.SelectBookedSeatsForJourney(journeyId,apartmentId));
  }

  public async Task<ResponseModel> SelectAllJourneysForSchedule(string scheduleId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnJourneyStationDto>>(await _BookingDbRepo.SelectAllJourneysForSchedule(scheduleId));
  }
}

