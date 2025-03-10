
using EmailSender.Configuration;
using EmailSender.Services;

namespace EmailSender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Configuration.AddJsonFile("appsettings.EmailSender.json", optional: false, reloadOnChange: true);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Регистриурем медиатР
            builder.Services.AddMediatR(x =>
            {
                // Указываем маркерный интерфейс
                // Он зарегистрирует хэндлеры
                x.RegisterServicesFromAssemblyContaining<Program>();
            });

            // Конфигурация для сообщений, отправляемых по почте
            builder.Services.Configure<MessageSettings>(builder.Configuration.GetSection(nameof(MessageSettings)));

            var rabbitUri = builder.Configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");

            var queueName = builder.Configuration.GetConnectionString("RabbitMqQueueName");
            ArgumentNullException.ThrowIfNull(queueName, "Rabbit Queue name can not be null!");

            builder.Services.AddSingleton<ISender, SenderByEmail>();

            builder.Services.AddHostedService<MainService>(provider => new MainService(
                rabbitUri,
                queueName,
                provider.GetService<ISender>()
                ));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.Run();
        }
    }
}
