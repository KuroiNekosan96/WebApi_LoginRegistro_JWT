﻿using System;
using System.Collections.Generic;

namespace WebApiCortPas.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? PasswordHash { get; set; }

    public string? Rol { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }
}
