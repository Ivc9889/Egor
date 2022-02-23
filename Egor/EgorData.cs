using Egor.Models;

namespace Egor
{
    public class EgorData
    {
		public static void Initialize(EgorContext context)
		{
			if (!context.Depts.Any())
			{
				context.Depts.AddRange(
					new Dept
                    {
						Code = "01.04.02",
						Name = "Прикладная математика и информатика"
					},
					new Dept
					{
						Code = "02.03.01",
						Name = "Математика и компьютерные науки"
					},
					new Dept
					{
						Code = "02.03.02",
						Name = "Фундаментальная информатика и информационные технологии"
					}
				);
				context.SaveChanges();
			}	
			if (!context.Profiles.Any())
            {
				context.Profiles.AddRange(
					new Profile
					{
						Name = "Математическое моделирование и вычислительная математика",
						DeptId = 1
					},
					new Profile
					{
						Name = "Математическое моделирование и вычислительная математика 2019",
						DeptId = 1
					},
					new Profile
					{
						Name = "Математический анализ и приложения",
						DeptId = 2
					},
					new Profile
					{
						Name = "Информатика и компьютерные науки (2021)",
						DeptId = 3
					}
				);
				context.SaveChanges();
			}
			if (!context.TypesProgram.Any())
            {
				context.TypesProgram.AddRange(
					new TypeProgram
					{
						Name = "Базовая",
						ProfileId = 1
					},
					new TypeProgram
					{
						Name = "Вариативная(Обязательные дисциплины)",
						ProfileId = 1
					},
					new TypeProgram
					{
						Name = "Обязательная часть",
						ProfileId = 2
					},
					new TypeProgram
					{
						Name = "Базовая",
						ProfileId = 3
					},
					new TypeProgram
					{
						Name = "Вариативная(Обязательные дисциплины)",
						ProfileId = 3
					},
					new TypeProgram
                    {
						Name = "Обязательная часть",
						ProfileId = 4
					},
					new TypeProgram
					{
						Name = "Дисциплины. Часть, формируемая участниками образовательных отношений",
						ProfileId = 4
					}
				);
				context.SaveChanges();
            }
			if (!context.Disciplines.Any())
            {
				context.Disciplines.AddRange(
					new Discipline
					{
						Code = "Б1.Б.3",
						Name = "непрерывные математические модели",
						TypeProgramId = 1,
						Content = "20181228-р49.pdf"
					},
					new Discipline
					{
						Code = "Б1.Б.4",
						Name = "иностранный язык",
						TypeProgramId = 1,
						Content = "20181029-з4.pdf"
					},
					new Discipline
					{
						Code = "Б1.В.ОД.1",
						Name = "прикладной функциональный анализ и интегральные уравнения",
						TypeProgramId = 2,
						Content = "20181129-п35.pdf"
					},
					new Discipline
					{
						Code = "Б1.В.ОД.5",
						Name = "методы монте-карло",
						TypeProgramId = 2,
						Content = "20181217-з39.pdf"
					},
                    new Discipline
					{
						Code = "Б1.О.2.8",
						Name = "численные методы решения уравнений математической физики",
						TypeProgramId = 3,
						Content = "20211118-а34.pdf"
					},
                    new Discipline
                    {
						Code = "Б1.Б.16",
						Name = "безопасность жизнедеятельности",
						TypeProgramId = 4,
						Content = "20201201-п3.pdf"
					},
					new Discipline
					{
						Code = "Б1.В.ОД.1",
						Name = "русский язык и культура речи",						
						TypeProgramId = 5,
						Content = "20201103-р29.pdf"
					},
                    new Discipline
                    {
						Code = "Б1.О.1.1",
						Name = "история",
						TypeProgramId = 6,
						Content = "20211117-а25.pdf"
					},
					new Discipline
					{ 
						Code = "Б1.О.3.3",
						Name = "кратные интегралы и ряды",
						TypeProgramId = 6,
						Content = "20211022-з28.pdf"
					},
					new Discipline
					{
						Code = "Б1.О.1.8",
						Name = "психология",
						TypeProgramId = 6,
						Content = "20211019-з24.pdf"
					},
					new Discipline
					{
						Code = "Б1.В.1.1",
						Name = "функциональный анализ",
						TypeProgramId = 7,
						Content = "20211213-м20.pdf"
					},
					new Discipline
					{
						Code = "Б1.В.1.3",
						Name = "введение в case-технологии",
						TypeProgramId = 7,
						Content = "20211210-м41.pdf"
					}
				);
				context.SaveChanges();
            }
        }
	}
}
