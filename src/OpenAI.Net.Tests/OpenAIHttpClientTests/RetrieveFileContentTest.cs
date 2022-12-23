﻿using Moq.Protected;
using Moq;
using OpenAI.Net.Models.Requests;
using System.Net;
using OpenAI.Net.Extensions;

namespace OpenAI.Net.Tests.OpenAIHttpClientTests
{
    internal class RetrieveFileContentTest
    {
        const string responseJson = @"
                                    {
                                        ""object"": ""file"",
                                        ""id"": ""file-GB1kRstIY1YqJQBZ6rkUVphO"",
                                        ""purpose"": ""fine-tune"",
                                        ""filename"": ""@file.png"",
                                        ""bytes"": 207,
                                        ""created_at"": 1671818085,
                                        ""status"": ""processed"",
                                        ""status_details"": null
                                    }";

        const string errorResponseJson = @"{""error"":{""message"":""an error occured"",""type"":""invalid_request_error"",""param"":""prompt"",""code"":""unsupported""}}";
     
        
        [TestCase(true, HttpStatusCode.OK, responseJson,null, Description = "Successfull Request")]
        [TestCase(false, HttpStatusCode.BadRequest, errorResponseJson, "an error occured", Description = "Failed Request")]
        public async Task Test_GetFiles(bool isSuccess,HttpStatusCode responseStatusCode,string responseJson,string errorMessage)
        {
            var imageEditRequest = new ImageEditRequest("a baby fish", new Models.FileContentInfo(new byte[] { 1 }, "image.png"));
            var formDataContent = imageEditRequest.ToMultipartFormDataContent();
            formDataContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
            formDataContent.Headers.ContentDisposition.FileName = "image.png";

            var jsonContent = new StringContent(responseJson);

            var res = new HttpResponseMessage { StatusCode = responseStatusCode, Content = isSuccess ? formDataContent : jsonContent };

            var handlerMock = new Mock<HttpMessageHandler>();
            string path = null;
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(() => res)
               .Callback<HttpRequestMessage, CancellationToken>((r, c) =>
               {
                   path = r.RequestUri.AbsolutePath;
               });

            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://api.openai.com") };

            var openAIHttpClient = new OpenAIHttpClient(httpClient);
            var response = await openAIHttpClient.RetrieveFileContent("1");

            Assert.That(response.IsSuccess, Is.EqualTo(isSuccess));
            Assert.That(response.Result != null, Is.EqualTo(isSuccess));
           // Assert.That(response.Result?.FileContent.Length > 0, Is.EqualTo(isSuccess));
            Assert.That(response.StatusCode, Is.EqualTo(responseStatusCode));
            Assert.That(response.Exception == null, Is.EqualTo(isSuccess));
            Assert.That(response.ErrorMessage == null, Is.EqualTo(isSuccess));
            Assert.That(response.ErrorResponse == null, Is.EqualTo(isSuccess));
            Assert.That(response.ErrorResponse?.Error?.Message, Is.EqualTo(errorMessage));
            Assert.That(response.ErrorResponse?.Error?.Type == null, Is.EqualTo(isSuccess));
            Assert.That(response.ErrorResponse?.Error?.Code == null, Is.EqualTo(isSuccess));
            Assert.That(response.ErrorResponse?.Error?.Param == null, Is.EqualTo(isSuccess));
            Assert.That(response.Result?.FileName == "image.png", Is.EqualTo(isSuccess));
            Assert.That(path, Is.EqualTo("/v1/files/1/content"),"Apth is incorrect");
        }
    }
}
