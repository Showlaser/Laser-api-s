﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security;

namespace Vote_API.Models.Helper
{
    public class ControllerResultHelper
    {
        public async Task<ActionResult<T?>> Execute<T>(Task<T> task)
        {
            try
            {
                return await task.WaitAsync(CancellationToken.None);
            }
            catch (InvalidDataException)
            {
                return new BadRequestResult();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }
            catch (DuplicateNameException)
            {
                return new ConflictResult();
            }
            catch (SecurityException)
            {
                return new UnauthorizedResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> Execute(Task task)
        {
            try
            {
                await task.WaitAsync(CancellationToken.None);
                return new OkResult();
            }
            catch (InvalidDataException)
            {
                return new BadRequestResult();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status304NotModified);
            }
            catch (DuplicateNameException)
            {
                return new ConflictResult();
            }
            catch (SecurityException)
            {
                return new UnauthorizedResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}